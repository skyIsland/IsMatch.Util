using IsMatch.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsMatch.Spider.Txt
{
    public class BiQuGe : CommonDownload
    {
        #region 构造

        public BiQuGe(Setting setting)
        {
            if(setting == null)
            {
                setting = new Setting();
            }

            base._Setting = setting;
        }

        #endregion

        #region 输出信息

        private Action<string> OutPutMsg = msg => NewLife.Log.XTrace.WriteLine(msg);

        #endregion

        //// "#list>dl>dd>a"

        //public void Download(string url)
        //{

        //}

        #region Override

        public static string ReplaceSpecial(string str)
        {
            str = str.Replace("天才一秒记住本站地址：[笔趣阁]", "")
                    .Replace("https://www.ibiquge.net/最快更新！无广告！", "")
                    .Replace("章节错误,点此报送(免注册),", "")
                    .Replace("报送后维护人员会在两分钟内校正章节内容,请耐心等待。", "");

            str = str.Replace("笔＆趣＆阁ｗww.ｂiｑuｇe.ｉnｆo", "");

            return str;
        }

        /// <summary>
        /// 给详情内容重新排序和去掉某些文字
        /// </summary>
        public override void FriendlyDetail()
        {
            //_DetailContext = _DetailContext.ToList()
            //foreach (var item in _DetailContext)
            //{
            //    item.Value = item.Value.Replace(@"                    天才一秒记住本站地址：[笔趣阁]
            //    https://www.ibiquge.net/最快更新！无广告！","");
            //}

            // TODO:两个字典，第二个字典如何按照第二个字典的key进行排序？
            //_DetailContext = _DetailContext.OrderBy(p =>
            //{
            //    var array = System.Text.Encoding.ASCII.GetBytes(p.Key);
            //    string asciiStr2 = null;
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        int asciiCode = (int)(array[i]);
            //        asciiStr2 += Convert.ToString(asciiCode);
            //    }
            //    return asciiStr2;
            //}).ToDictionary(o => o.Key, p => p.Value);

            var newDic = new Dictionary<string, string>();
            foreach (var detail in _ListUrl)
            {
                var detailKey = detail.Key;
                if (_DetailContext.ContainsKey(detailKey))
                {
                    newDic.Add(detailKey, ReplaceSpecial(_DetailContext[detailKey]));
                }
            }

            _DetailContext = newDic;
        }


        #endregion

        #region Main Function

        public void Start()
        {
            NewLife.Log.XTrace.UseConsole();

            //base._UrlStart = "https://www.ibiquge.net";
            //base._Url = "https://www.ibiquge.net/39_39483/";

            //base._UrlStart = "https://www.biquge.info/32_32870/";
            //base._Url = "https://www.biquge.info/32_32870/";

            // 获取列表
            base.GetList(base._Setting.ListRule, (htmlCollect) =>
            {
                foreach (var dd in htmlCollect)
                {
                    if (!_ListUrl.ContainsKey(dd.TextContent))
                    {
                        base._ListUrl.Add(dd.TextContent, dd.GetAttribute("href"));
                    }
                }
            });

            // 获取详情
            base.GetDetail(base._Setting.DetailRule, (elemt, title) =>
            {
                if (!base._DetailContext.ContainsKey(title))
                {
                    base._DetailContext.Add(title, elemt?.TextContent);
                }
            },
             OutPutMsg
            );

            // 输出文本文件
            base.OutPutTxt(OutPutMsg);
        }


        #endregion

    }
}
