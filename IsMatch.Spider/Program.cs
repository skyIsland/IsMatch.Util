using IsMatch.Spider.Txt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsMatch.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //JinXuLiangSpider();
            //DouBanSpider();

            BiQuGeSpider();

            Console.ReadKey();
        }

        private static void DouBanSpider()
        {
            NewLife.Log.XTrace.UseConsole();
            var factory = new DouBan.DouBan();
            factory.DownloadAllImage();
        }

        private static void JinXuLiangSpider()
        {
            var courseSource = new CourseSource();
            courseSource.Work();
        }

        private static void BiQuGeSpider()
        {
            //File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Txt", "Rule.json"), NewLife.Serialization.JsonHelper.ToJson(new List<Rule> { new Rule(), new Rule() }));
            //return;
            // 读取配置
            var setting =
               NewLife.Serialization.JsonHelper.ToJsonEntity<Setting>(
                   File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Txt", "Setting.json")));

            //var listRule = NewLife.Serialization.JsonHelper.ToJsonEntity<List<Rule>>(
            //       File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Txt", "Rule.json")));

            var listRule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rule>>(
                   File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Txt", "Rule.json")));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (setting != null && listRule != null && listRule.Any())
            {
                var rule = listRule.FirstOrDefault(p => p.Id == setting.RuleId);
                rule.TxtIndexUrl = string.Format(rule.TxtIndexUrl, setting.StoryId);
                rule.UrlStart = string.Format(rule.UrlStart, setting.StoryId);

                BiQuGe.Start(rule);
            }
        }
    }

    #region Common

    public class CmdReader
    {
        public static string ReadLine(string tipText, Func<string, bool> validate = null)
        {
            var input = string.Empty;
            while (string.IsNullOrWhiteSpace(input) || (validate != null && !validate(input)))
            {
                Console.WriteLine(tipText);
                input = Console.ReadLine();
            }
            return input;
        }
    }

    #endregion

}
