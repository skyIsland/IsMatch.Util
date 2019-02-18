using System;

namespace IsMatch.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //JinXuLiangSpilder();
            DouBanSpider();
            Console.ReadKey();
        }

        private static void DouBanSpider()
        {
            NewLife.Log.XTrace.UseConsole();
            var factory = new DouBan.DouBan();
            factory.DownloadAllImage();
        }

        private static void JinXuLiangSpilder()
        {
            var courseSource = new CourseSource();
            courseSource.Work();
        }
    }
}
