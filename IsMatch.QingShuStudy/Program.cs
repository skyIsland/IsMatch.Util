using System;

namespace IsMatch.QingShuStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*
               1.获取每个课程的所有视频时长
               2.开始学习 
                    https://api.qingshuxuetang.com/v6_9/behavior/trackPage?_t=1560748629063
                    https://degree.qingshuxuetang.com/gxun/Student/UploadStudyRecordBegin?_t=1560748631976
               3.中途不断更新学习时间 
                    https://degree.qingshuxuetang.com/gxun/Student/UploadStudyRecordBegin?_t=1560748631976
               4.直到时间到达该视频的结束时间，更改视频id和时长继续
             */

            Console.ReadKey();
        }
    }
}
