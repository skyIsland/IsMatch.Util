using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using IsMatch.CnArticleSubscribe.Config;
using IsMatch.CnArticleSubscribe.Helper;
using IsMatch.CnArticleSubscribe.Models;
using NewLife.Log;
using NewLife.Serialization;
using Polly;
using Polly.Retry;

namespace IsMatch.Cnarticlesubscribe
{
    class Program
    {
        private static string BlogDataUrl = "https://www.cnblogs.com";
        private static string BlogPageUrl = "https://www.cnblogs.com/mvc/AggSite/PostList.aspx"; 
        private static readonly Stopwatch _sw = new Stopwatch();
        private static List<Article> PreviousArticles;
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
            if (args.Length > 0)
            {
                SendMail(args[0]);
            }
            else
            {
                new Thread(WorkStart).Start();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        static void Init()
        {
            PreviousArticles = new List<Article>();

            // 初始化重试器
            _retryThreeTimesPolicy =
                Policy
                    .Handle<Exception>()
                    .Retry(3, (ex, count) =>
                    {
                        NewLife.Log.XTrace.Log.Error($"excuted Failed! Retry {count}");
                        NewLife.Log.XTrace.Log.Error($"Exeption from {ex.GetType().Name}, message:{ex.Message}");
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
            _recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 20, 0);

            // 初始化抓取页数
            _maxPageNo = 1;

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
                        PreviousArticles.AddRange(res);// todo:抓取一天之后PreviousArticles会占用多少内存
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
                    Thread.Sleep(60 * 5 * 1000);
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

                string html = HttpHelper.GetString(new Uri(blogUrl), isGet, parame);

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
                    Article article = new Article()
                    {
                        Title = title,
                        Url = url,
                        Summary = summary,
                        Author = author,
                        PublishTime = DateTime.Parse(publishTime),
                        CommentCount = commentCount,
                        ViewCount = viewCount,
                        InputTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    ret.Add(article);

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

                List<Article> articles = GetListArticle();

                // 入库操作
                foreach (var artilce in articles)
                {
                    var hasArticleByUrl = PreviousArticles.Find(p => p.Url == artilce.Url);

                    // 已存在该文章，则更新文章的评论量和阅读量
                    if(hasArticleByUrl != null)
                    {
                        repeatCount++;
                        hasArticleByUrl.CommentCount = artilce.CommentCount;
                        hasArticleByUrl.ViewCount = artilce.ViewCount;
                        hasArticleByUrl.UpdateTime = DateTime.Now;
                    }
                    else
                    {
                        // 反之则直接插入本次抓取记录
                        PreviousArticles.Add(artilce);
                    }
                }

                // 持久化本次抓取数据到文本 以便于异常退出恢复之后不出现重复数据
                File.WriteAllText(_tmpFilePath, NewLife.Serialization.JsonHelper.ToJson(PreviousArticles));

                _sw.Stop();

                // 统计信息
                NewLife.Log.XTrace.Log.Info($"获取数据成功,耗时:{_sw.ElapsedMilliseconds}ms,文章数量:{articles.Count},文章重复次数:{repeatCount},有效文章数量:{articles.Count - repeatCount}");

                // 生成Txt和发送邮件
                if ((DateTime.Now - _recordTime).TotalHours >= 24)
                {
                    BuildArticleTxt(PreviousArticles.OrderByDescending(p => p.CommentCount).ThenByDescending(p => p.ViewCount).ToList());
                    NewLife.Log.XTrace.Log.Info($"准备发送邮件，记录时间:{_recordTime:yyyy-MM-dd HH:mm:ss}");
                    SendMail();
                    _recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 20, 0);
                    NewLife.Log.XTrace.Log.Info($"记录时间已更新:{_recordTime:yyyy-MM-dd HH:mm:ss}");

                    // ImportDb
                    ImportDb();
                }
                //ImportDb();
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
        /// <param name="blogFileName">指定文件名</param>
        static void SendMail(string blogFileName = "")
        {
            if (string.IsNullOrWhiteSpace(blogFileName))
            {
                blogFileName = $"博客园首页文章聚合-{DateTime.Now:yyyy-MM-dd}.txt";
            }

            string blogFilePath = Path.Combine(_baseDir, "articles", blogFileName);

            if (!File.Exists(blogFilePath))
            {
                NewLife.Log.XTrace.Log.Error("未发现文件记录，无法发送邮件，所需文件名：" + blogFileName);
                return;
            }

            // 邮件正文
            string mailContent = BuildEmailContent(PreviousArticles.OrderByDescending(p => p.CommentCount).ThenByDescending(p => p.ViewCount).ToList());

            // 附件内容
            string blogFileContent = File.ReadAllText(blogFilePath);

            // 发送邮件
            MailHelper.SendMail(_mailConfig, _mailConfig.ReceiveList, "博客园首页文章聚合",
                $"傻大蒙，你好。博客园首页文章聚合【{_recordTime:yyyy-MM-dd}】", mailContent, Encoding.UTF8.GetBytes(blogFileContent),
                blogFileName);

            // 清空昨天的缓存
            PreviousArticles.RemoveAll(p => true);
            NewLife.Log.XTrace.Log.Info($"{blogFileName},文件已发送。");
        }

        /// <summary>
        /// 生成文章txt文件
        /// </summary>
        /// <param name="articleList"></param>
        static void BuildArticleTxt(List<Article> articleList)
        {
            string blogFileName = $"博客园首页文章聚合-{DateTime.Now:yyyy-MM-dd}.txt";
            string blogFilePath = Path.Combine(_baseDir, "articles", blogFileName);

            FileStream fs = new FileStream(blogFilePath, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            int i = 1;
            foreach (var artilce in articleList)
            {
                sw.WriteLine($"序号：{i}");
                sw.WriteLine($"标题：{artilce.Title}");
                sw.WriteLine($"网址：{artilce.Url}");
                sw.WriteLine($"摘要：{artilce.Summary}");
                sw.WriteLine($"作者：{artilce.Author}");
                sw.WriteLine($"发布时间：{artilce.PublishTime:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine($"评论量：{artilce.CommentCount}");
                sw.WriteLine($"阅读量：{artilce.ViewCount}");
                sw.WriteLine($"抓取入库时间：{artilce.InputTime:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine($"抓取更新时间：{artilce.UpdateTime:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine("--------------华丽的分割线---------------");

                i++;
            }

            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 生成邮件正文
        /// </summary>
        /// <param name="articleList"></param>
        static string BuildEmailContent(List<Article> articleList)
        {
            int i = 1;
            string emailContent = "";
            foreach (var artilce in articleList)
            {
                artilce.FilterHtml();

                emailContent += $"<span style=\"font-weight:bold;\">序号</span>：{i}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">标题</span>：{artilce.Title}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">网址</span>：{artilce.Url}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">摘要</span>：{artilce.Summary}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">作者</span>：{artilce.Author}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">发布时间</span>：{artilce.PublishTime:yyyy-MM-dd HH:mm:ss}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">评论量</span>：{artilce.CommentCount}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">阅读量</span>：{artilce.ViewCount}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">抓取入库时间</span>：{artilce.InputTime:yyyy-MM-dd HH:mm:ss}";
                emailContent += "<br>";
                emailContent += $"<span style=\"font-weight:bold;\">抓取更新时间</span>：{artilce.UpdateTime:yyyy-MM-dd HH:mm:ss}";
                emailContent += "<br>";
                emailContent += "<span style=\"font-weight:bold;color:red;\">--------------华丽的分割线---------------</span>";
                emailContent += "<br>";

                i++;
            }

            return emailContent;
        }

        static void SendEmailTest()
        {
            // 发送邮件
            MailHelper.SendMail(_mailConfig, _mailConfig.ReceiveList, "Test",
              "Test", "TestContent");

            // 清空昨天的缓存
            PreviousArticles.RemoveAll(p => true);
            //NewLife.Log.XTrace.Log.Info($"{blogFileName},文件已发送。");
        }

        static void ImportDb()
        {
            var postString = PreviousArticles.ToJson();
            var result = HttpHelper.GetString(new Uri("http://localhost:51086/ArticleManager/ArticleList/Import"), false, postString);
        }
    }
}
