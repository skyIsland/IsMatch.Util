using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace IsMatch.Spider.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// 网页源代码
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string WebClientGetHtml(string address, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding("UTF-8");
            }
            var str = string.Empty;
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Encoding = encoding;
                    str = wc.DownloadString(address);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"请求{address}发生错误,原因{e.Message}");
            }

            return str;
        }

        /// <summary>
        /// 获取网页源代码(ContentTypet特殊处理)
        /// </summary>
        /// <param name="uri">爬虫URL地址</param>
        /// <param name="isGet">是否Get请求</param>
        /// <param name="parame">parame</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public static string WebRequestGetHtml(Uri uri, bool isGet = true, string parame = null, string proxy = null, Encoding encoding = null)
        {
            var pageSource = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Accept = "*/*";

                // 加快载入速度
                request.ServicePoint.Expect100Continue = false;

                // 禁止Nagle算法加快载入速度
                request.ServicePoint.UseNagleAlgorithm = false;

                // 禁止缓冲加快载入速度
                request.AllowWriteStreamBuffering = false;

                // 定义gzip压缩页面支持
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

                // 定义文档类型及编码 设置与否并不影响博客园接收参数 2018年12月2日00:07:06 设置与否真的影响博客园接收参数！
                request.ContentType = string.IsNullOrEmpty(parame) ? "application/x-www-form-urlencoded" : "application/json; charset=UTF-8";
                //request.ContentType = "application/x-www-form-urlencoded";

                // 禁止自动跳转
                request.AllowAutoRedirect = false;

                // 设置User-Agent，伪装成Google Chrome浏览器
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

                // 定义请求超时时间为5秒
                request.Timeout = 5000;

                // 启用长连接
                request.KeepAlive = true;

                // 定义请求方式为GET    
                request.Method = isGet ? "GET" : "POST";

                // 定义来源
                request.Referer = uri.ToString();

                // 加入参数
                if (!string.IsNullOrEmpty(parame))
                {
                    byte[] byteData = Encoding.UTF8.GetBytes(parame);
                    request.ContentLength = byteData.Length;
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(byteData, 0, byteData.Length);
                    }
                }

                // 设置代理服务器IP，伪装请求地址
                if (proxy != null) request.Proxy = new WebProxy(proxy);
                //request.CookieContainer = this.CookiesContainer;//附加Cookie容器

                // 定义最大连接数
                request.ServicePoint.ConnectionLimit = int.MaxValue;

                // 获取请求响应
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // gzip解压
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            if (encoding == null) encoding = Encoding.UTF8;
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                pageSource = reader.ReadToEnd();
                            }
                        }
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))// deflate解压
                    {
                        using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            if (encoding == null) encoding = Encoding.UTF8;
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                pageSource = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (Stream stream = response.GetResponseStream())//原始
                        {
                            if (encoding == null) encoding = Encoding.UTF8;
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                pageSource = reader.ReadToEnd();
                            }
                        }
                    }
                }
                request.Abort();
            }
            catch (Exception ex)
            {
                pageSource = $"[error]请求源代码发生错误，原因:{ex.Message}";
            }
            return pageSource;
        }

    }
}
