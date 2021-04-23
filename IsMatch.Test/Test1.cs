using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using IsMatch.Upload;
using Qiniu.Storage;
using Xunit;

namespace IsMatch.Test
{
    public class Test1
    {
        [Fact]
        public void FormUplaod()
        {
            var config = new Config
            {
                // 设置上传区域
                Zone = Zone.ZoneCnEast,

                // 设置 http 或者 https 上传
                UseHttps = true,
                UseCdnDomains = true,
                ChunkSize = ChunkUnit.U512K
            };
            string ak = "", sk = "";
            var upload = new QiNiuUpload(config, ak, sk);
            var uploadResult = upload.FormUpload("0060lm7Tgy1fistv13d8zj30k00zkgne.jpg");
            Assert.Contains("hash", uploadResult);
        }

        [Fact]
        public void NetworksocketTest()
        {
            // 启动
            IsMatch.NetworkSocket.Startup.Main();

            string url = "http://192.168.8.58:1212/Home/GetUsers";
            string result;

            using (var wc = new WebClient())
            {
                result = wc.DownloadString(url);
            }

            Assert.NotNull(result);
        }       
    }
}
