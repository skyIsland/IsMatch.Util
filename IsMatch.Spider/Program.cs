using IsMatch.Spider.Txt;
using System;

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
            new BiQuGe().Start();
        }
    }
}
