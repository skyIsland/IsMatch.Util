using IsMatch.Spider.Txt;
using System;
using System.IO;

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
            // 读取配置

            var setting =
               NewLife.Serialization.JsonHelper.ToJsonEntity<Setting>(
                   File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Txt", "Setting.json")));

            new BiQuGe(setting).Start();
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
