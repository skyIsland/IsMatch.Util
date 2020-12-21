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
               {"schoolId":"7","classId":"419","courseId":"2433","contentId":"kcjs_21","contentType":11,"periodId":"17","position":0}
               3.中途不断更新学习时间 
                    https://degree.qingshuxuetang.com/gxun/Student/UploadStudyRecordContinue?_t=1608563471421
            {"recordId":29154531,"position":358,"timeOutConfirm":false} position步长121 
               4.直到时间到达该视频的结束时间，更改视频id和时长继续
             */

            Console.ReadKey();
        }
    }
}
