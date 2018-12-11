using System;
using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace IsMatch.Upload
{
    public class QiNiuUpload
    {
        private Config _Config;
        private string _AccessKey;
        private string _SecretKey;

        public QiNiuUpload(Config config, string ak, string sk)
        {
            _Config = config;
            _AccessKey = ak;
            _SecretKey = sk;
        }

        public string FormUpload(string filePath)
        {
            Mac mac = new Mac(_AccessKey, _SecretKey);

            // 上传文件名
            string key = "key";

            // 存储空间名
            string Bucket = "file";

            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();

            // 设置要上传的目标空间
            putPolicy.Scope = Bucket;

            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);

            // 文件上传完毕后，在多少天后自动被删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传token
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            // 表单上传
            FormUploader target = new FormUploader(_Config);
            var result = target.UploadFile(filePath, key, token, null).Result;
            return result.ToString();
        }
    }
}
