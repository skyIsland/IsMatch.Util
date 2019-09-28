using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using IsMatch.Spider.Txt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IsMatch.Spider.Common
{
    public class CommonDownload
    {
        public Rule _Rule;

        public Dictionary<string, string> _ListUrl = new Dictionary<string, string>();

        public Dictionary<string, string> _DetailContext = new Dictionary<string, string>();

        public string _BaseDir = "../OutPut/";

        /// <summary>
        /// 获取网页源代码并转换为IHtmlDocument
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public virtual IHtmlDocument GetHtmlDocumet(string address, Encoding encoding = null)
        {
            if(!string.IsNullOrEmpty(this._Rule.CharCode)) encoding = Encoding.GetEncoding(this._Rule.CharCode);

            var resultStr = HttpHelper.WebRequestGetHtml(new Uri(address), encoding: encoding);
            return new HtmlParser().ParseDocument(resultStr);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="selectors"></param>
        /// <param name="htmlCollection"></param>
        public virtual void GetList(string selectors, Action<IHtmlCollection<IElement>> htmlCollection, Action<string> WriteLog = null)
        {
            var document = GetHtmlDocumet(_Rule.TxtIndexUrl);
            var ddList = document.QuerySelectorAll(selectors);

            htmlCollection?.Invoke(ddList);

            //foreach (var dd in ddList)
            //{
            //    _ListUrl.Add(dd.TextContent, dd.GetAttribute("herf"));
            //}
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="selectors"></param>
        /// <param name="htmlCollection"></param>
        /// <param name="WriteLog"></param>
        public virtual void GetDetail(string selectors, Action<IElement, string> htmlCollection, Action<string> WriteLog = null)
        {
            if (_ListUrl != null && _ListUrl.Count > 0)
            {


                #region 开线程下载 10个链接开一个线程
                int maxCountThread = 100;
                var threadCount = (int)Math.Ceiling((double)_ListUrl.Count / maxCountThread);
                Task[] tasks = new Task[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    var data = _ListUrl.ToList().Skip(i * maxCountThread).Take(maxCountThread).ToList();
                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        foreach (var detailUrl in data)
                        {
                            var document = GetHtmlDocumet(_Rule.UrlStart + detailUrl.Value);
                            var detailHtml = document.QuerySelector(selectors);

                            if (detailHtml != null)
                            {
                                htmlCollection?.Invoke(detailHtml, detailUrl.Key);

                                WriteLog?.Invoke($"{detailUrl.Key}获取成功！");                               
                            }
                            else
                            {
                                WriteLog?.Invoke($"{detailUrl.Key}获取失败！");
                            }
                        }

                        //foreach (var list in data)
                        //{
                        //    // 单章详情页内容
                        //    var detailHtml = GetHtmlDocumet(list.Value);

                        //    var detailStr =
                        //}
                    });
                }
                Task.WaitAll(tasks);
                #endregion
                //foreach (var detailUrl in _ListUrl)
                //{
                //    var document = GetHtmlDocumet(_UrlStart + detailUrl.Value);
                //    var detailHtml = document.QuerySelector(selectors);

                //    htmlCollection?.Invoke(detailHtml, detailUrl.Key);

                //    WriteLog?.Invoke($"{detailUrl.Key}获取成功！");
                //}
            }
            else
            {
                WriteLog?.Invoke("列表页为空，无法继续获取详情页内容");
            }
        }

        /// <summary>
        /// 输出文本文件
        /// </summary>
        /// <param name="WriteLog"></param>
        public virtual void OutPutTxt(Action<string> WriteLog = null)
        {
            if (_DetailContext != null && _DetailContext.Count > 0)
            {
                var title = Guid.NewGuid().ToString().Substring(0, 6);

                // 获取应用程序所在目录
                Type type = (new Program()).GetType();
                _BaseDir = Path.GetDirectoryName(type.Assembly.Location);

                // 检查工作目录
                if (!Directory.Exists(_BaseDir))
                {
                    Directory.CreateDirectory(_BaseDir);
                }

                FriendlyDetail();

                File.WriteAllText(_BaseDir + "/" + title + ".txt", string.Join("", _DetailContext.ToList()
                    .Select(p => FriendTitle(p.Key) + p.Value)));

                WriteLog("文本文件输出完成!");
            }
            else
            {
                WriteLog("详情页内容为空，无法输出文本文件!");
            }
        }

        public virtual void FriendlyDetail()
        {

        }

        public string FriendTitle(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.ToInt(-1) > -1)
            {
                str = "第" + str + "章";
            }

            return str;
        }
    }
}
