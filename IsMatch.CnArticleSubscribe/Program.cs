using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using IsMatch.CnArticleSubscribe.Config;
using IsMatch.CnArticleSubscribe.Helper;
using IsMatch.CnblogSubscribe.Models;
using NewLife.Log;
using Polly;
using Polly.Retry;

namespace IsMatch.Cnarticlesubscribe
{
    class Program
    {
        private static string BlogDataUrl = "https://www.cnblogs.com";
        private static string BlogPageUrl = "https://www.cnblogs.com/mvc/AggSite/PostList.aspx"; 
        private static readonly Stopwatch _sw = new Stopwatch();
        private static readonly List<Article> PreviousArticles = new List<Article>();
        private static MailConfig _mailConfig;
        private static string _baseDir;
        private static RetryPolicy _retryThreeTimesPolicy;
        private static string _tmpFilePath;
        private static int _maxPageNo;

        private static DateTime _recordTime;


        static void Main(string[] args)
        {
            // 接管控制台输出日志
            XTrace.UseConsole();

            Console.WriteLine("start work...");

            Init();

            // start
            new Thread(WorkStart).Start();
            Console.ReadKey();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        static void Init()
        {

            // 初始化重试器
            _retryThreeTimesPolicy =
                Policy
                    .Handle<Exception>()
                    .Retry(3, (ex, count) =>
                    {
                        NewLife.Log.XTrace.Log.Error($"xcuted Failed! Retry {count}");
                        NewLife.Log.XTrace.Log.Error($"Exeption from {ex.GetType().Name}");
                    });

            // 获取应用程序所在目录
            Type type = (new Program()).GetType();
            _baseDir = Path.GetDirectoryName(type.Assembly.Location);

            // 检查工作目录
            if (!Directory.Exists(Path.Combine(_baseDir, "articles")))
            {
                Directory.CreateDirectory(Path.Combine(_baseDir, "articles"));
            }

            // 初始化记录时间
            _recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);

            // 初始化抓取页数
            _maxPageNo = 2;

            // 初始化邮件配置
            _mailConfig =
                NewLife.Serialization.JsonHelper.ToJsonEntity<MailConfig>(
                    File.ReadAllText(Path.Combine(_baseDir, "Config", "Mail.json")));

            // 加载最后一次成功获取数据缓存
            _tmpFilePath = Path.Combine(_baseDir, "articles", "cnarticles.tmp");
            if (File.Exists(_tmpFilePath))
            {
                try
                {
                    var data = File.ReadAllText(_tmpFilePath);
                    var res = NewLife.Serialization.JsonHelper.ToJsonEntity<List<Article>>(data);
                    if (res != null)
                    {
                        PreviousArticles.AddRange(res);
                    }
                }
                catch (Exception e)
                {
                    NewLife.Log.XTrace.Log.Error("缓存数据加载失败，本次将弃用！详情:" + e.Message);
                    File.Delete(_tmpFilePath);
                }
            }
        }

        /// <summary>
        /// 主任务
        /// </summary>
        static void WorkStart()
        {
            try
            {
                while (true)
                {
                    _retryThreeTimesPolicy.Execute(Work);

                    // 每五分钟执行一次
                    Thread.Sleep(300000);
                }
            }
            catch (Exception e)
            {
                NewLife.Log.XTrace.Log.Error($"Excuted Failed,Message: ({e.Message})");

            }
        }

        static List<Article> GetListArticle()
        {
            var ret = new List<Article>();
            for (int i = 0; i < _maxPageNo; i++)
            {
                string blogUrl = BlogDataUrl,parame = "";
                bool isGet = true;
                if (i > 0)
                {
                    blogUrl = BlogPageUrl;
                    parame = "{\"CategoryType\":\"SiteHome\",\"ParentCategoryId\":0,\"CategoryId\":808,\"PageIndex\":" + (i + 1) + ",\"TotalPostCount\":4000,\"ItemListActionName\":\"PostList\"}";
                    isGet = false;
                }

                string html = HttpHelper.GetString(new Uri(blogUrl), isGet,parame);

                HtmlParser parser = new HtmlParser();
                IHtmlDocument doc = parser.Parse(html);

                // 获取所有文章数据项
                var itemBodys = doc.QuerySelectorAll(".post_item_body");

                foreach (var itemBody in itemBodys)
                {
                    // 标题元素
                    IElement titleElem = itemBody.QuerySelector("h3>a");

                    // 获取标题
                    string title = titleElem?.TextContent;

                    // 获取url
                    string url = titleElem?.GetAttribute("href");

                    // 摘要元素
                    IElement summaryElem = itemBody.QuerySelector("p.post_item_summary");

                    // 获取摘要
                    string summary = summaryElem?.TextContent.Replace("\r\n", "").Trim();

                    // 数据项底部元素
                    IElement footElem = itemBody.QuerySelector("div.post_item_foot");

                    // 获取作者
                    string author = footElem?.QuerySelector("a").TextContent;

                    // 获取文章发布时间
                    string publishTime = Regex.Match(footElem?.TextContent, "\\d+-\\d+-\\d+ \\d+:\\d+").Value;

                    // 获取评论量
                    int commentCount = (footElem.QuerySelector("span.article_comment").TextContent?.Replace("评论(", "").Replace(")", "")).ToInt();

                    // 获取阅读量
                    int viewCount = (footElem.QuerySelector("span.article_view").TextContent?.Replace("阅读(", "").Replace(")", "")).ToInt();


                    // 组装博客对象
                    Article blog = new Article()
                    {
                        Title = title,
                        Url = url,
                        Summary = summary,
                        Author = author,
                        PublishTime = DateTime.Parse(publishTime),
                        CommentCount = commentCount,
                        ViewCount = viewCount
                    };
                    ret.Add(blog);

                    /*Console.WriteLine($"标题：{title}");
                    Console.WriteLine($"网址：{url}");
                    Console.WriteLine($"摘要：{summary}");
                    Console.WriteLine($"作者：{author}");
                    Console.WriteLine($"发布时间：{publishTime}");
                    Console.WriteLine("--------------华丽的分割线---------------");*/
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取到文章之后处理
        /// </summary>
        static void Work()
        {
            try
            {
                _sw.Reset();
                _sw.Start();

                // 重复数量统计
                int repeatCount = 0;

                string blogFileName = $"cnarticles-{DateTime.Now:yyyy-MM-dd}.txt";
                string blogFilePath = Path.Combine(_baseDir, "articles", blogFileName);
                FileStream fs = new FileStream(blogFilePath, FileMode.Append, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                List<Article> articles = GetListArticle();
                // 去重
                foreach (var artilce in articles)
                {
                    if (PreviousArticles.Any(b => b.Url == artilce.Url))
                    {
                        repeatCount++;
                    }
                    else
                    {
                        sw.WriteLine($"标题：{artilce.Title}");
                        sw.WriteLine($"网址：{artilce.Url}");
                        sw.WriteLine($"摘要：{artilce.Summary}");
                        sw.WriteLine($"作者：{artilce.Author}");
                        sw.WriteLine($"发布时间：{artilce.PublishTime:yyyy-MM-dd HH:mm}");
                        sw.WriteLine($"评论量：{artilce.CommentCount}");
                        sw.WriteLine($"阅读量：{artilce.ViewCount}");
                        sw.WriteLine("--------------华丽的分割线---------------");
                    }

                }
                sw.Close();
                fs.Close();

                // 清除上一次抓取数据记录
                PreviousArticles.Clear();

                // 加入本次抓取记录
                PreviousArticles.AddRange(articles);

                // 持久化本次抓取数据到文本 以便于异常退出恢复之后不出现重复数据
                File.WriteAllText(_tmpFilePath, NewLife.Serialization.JsonHelper.ToJson(articles));

                _sw.Stop();

                // 统计信息
                NewLife.Log.XTrace.Log.Info($"获取数据成功,耗时:{_sw.ElapsedMilliseconds}ms,文章数量:{articles.Count},文章重复次数:{repeatCount},有效文章数量:{articles.Count - repeatCount}");

                // 发送邮件
                if ((DateTime.Now - _recordTime).TotalHours >= 24)
                {
                    NewLife.Log.XTrace.Log.Info($"准备发送邮件，记录时间:{_recordTime:yyyy-MM-dd HH:mm:ss}");
                    SendMail();
                    _recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
                    NewLife.Log.XTrace.Log.Info($"记录时间已更新:{_recordTime:yyyy-MM-dd HH:mm:ss}");
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _sw.Stop();
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        static void SendMail()
        {
            string blogFileName = $"cnarticles-{_recordTime:yyyy-MM-dd}.txt";
            string blogFilePath = Path.Combine(_baseDir, "articles", blogFileName);

            if (!File.Exists(blogFilePath))
            {
                NewLife.Log.XTrace.Log.Error("未发现文件记录，无法发送邮件，所需文件名：" + blogFileName);
                return;
            }

            // 邮件正文
            string mailContent = "";
            FileStream mailFs = new FileStream(blogFilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(mailFs, Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                mailContent += sr.ReadLine() + "<br/>";
            }
            sr.Close();
            mailFs.Close();

            // 附件内容
            string blogFileContent = File.ReadAllText(blogFilePath);

            // 发送邮件
            MailHelper.SendMail(_mailConfig, _mailConfig.ReceiveList, "CnarticlesubscribeTool",
                $"傻大蒙，你好。博客园首页文章聚合【{_recordTime:yyyy-MM-dd}】", mailContent, Encoding.UTF8.GetBytes(blogFileContent),
                blogFileName);

            NewLife.Log.XTrace.Log.Info($"{blogFileName},文件已发送。");
        }
    }
}
