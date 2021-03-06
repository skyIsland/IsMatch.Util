﻿using IsMatch.NfxTest.LeetCode;
using IsMatch.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IsMatch.NfxTest
{
    public class MyClass1
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class MyClass2
    {
        public string Guid { get; set; }

        public string Title { get; set; }

        public static (string name, int id) GetList(List<MyClass1> classList)
        {
            var obj = classList.FirstOrDefault() ?? new MyClass1();
            return (obj.Name, obj.Id);
        }

        public static void GetList()
        {
            var obj = ("1", 1, new MyClass1());
            string i = obj.Item1;
            int j = obj.Item2;
            MyClass1 k = obj.Item3;
        }

        public override string ToString()
        {
            return Guid + "," + Title;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(string.Join(",", YieldTest()));
            //TestRef();
            //Test1();

            //Test2();

            //Test3();

            //Test4();

            //Test5();

            //Test6();

            //Test7();

            //Test8();

            //Test9();

            //Test10();

            //Console.WriteLine(ExecuteFormula("1+1*7"));             

            //Console.WriteLine(GetTimePoint(new List<DateTime>(){
            //    new DateTime(2019,8,1,16,0,0),
            //    new DateTime(2019,8,1,10,0,0),
            //    new DateTime(2019,8,1,11,0,0),
            //    new DateTime(2019,8,1,12,0,0),
            //    new DateTime(2019,8,1,14,0,0),
            //    new DateTime(2019,8,1,15,0,0)
            //}));

            //ArraySubject.TestControl();

            //var list = new List<int> { 5, 22, 1, 44, 11, 7, 9 };

            //Sorting.SortingMethod.BubbleSort(list);

            //Sorting.SortingMethod.SelectionSort(list);

            //Sorting.SortingMethod.InsertionSort(list);

            //Console.WriteLine(string.Join(",", Sorting.SortingMethod.MergeSort(list)));

            //TestWhileSetValue();

            //ExceptionTest();

            //Console.WriteLine(StampToDateTime("1569290406").ToString("yyyy-MM-dd HH:mm:ss"));

            GetDateTimeRanges_Test();

            Console.ReadKey();
        }

        public static void GetDateTimeRanges_Test()
        {
            // 当月
            var startTime = new DateTime(2021, 6, 3);
            var endTime = new DateTime(2021, 6, 5);
            var result = DateTimeHelper.GetDateTimeRanges(startTime, endTime, 0);
            FormatWrite(result);            

            // 跨一月
            startTime = new DateTime(2021, 6, 3);
            endTime = new DateTime(2021, 7, 5);
            result = DateTimeHelper.GetDateTimeRanges(startTime, endTime, 0);
            FormatWrite(result);

            // 跨两月
            startTime = new DateTime(2021, 6, 3);
            endTime = new DateTime(2021, 8, 5);
            result = DateTimeHelper.GetDateTimeRanges(startTime, endTime, 0);
            FormatWrite(result);

            // 跨五月
            startTime = new DateTime(2021, 6, 3);
            endTime = new DateTime(2021, 11, 5);
            result = DateTimeHelper.GetDateTimeRanges(startTime, endTime, 0);
            FormatWrite(result);
        }

        public static void FormatWrite(Dictionary<string, TimeRange> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key}:{item.Value.StartTime}-{item.Value.EndTime}");
            }

            Console.WriteLine("\n");
        }

        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        private static void TestWhileSetValue()
        {
            int i = 0;
            while ((i = 10) < 30)
            {
                Console.WriteLine(i);
            }
        }

        private static void TestRef()
        {
            var list = new List<MyClass2> {
                new MyClass2 { Guid = Guid.NewGuid().ToString(), Title = "guid1" },
                new MyClass2 { Guid = Guid.NewGuid().ToString(), Title = "guid2" }
            };

            var obj = list.FirstOrDefault(p => p.Title == "guid1");
            obj.Title = "guid3";

            Console.WriteLine(string.Join(",", list.Select(p => p.ToString())));
        }

        private static void Test1()
        {
            AssemblyTest.GetEntityType("MailHelper");
        }

        public static void Test2()
        {
            List<int> intList = new List<int> {1, 5, 2, 66, 22, 12, 34, 4, 9, 3};

            Console.WriteLine($"原数组元素如下:{string.Join(",", intList)}");

            int length = intList.Count - 1;

            // 排序趟数
            for (int i = 0; i < length; i++)
            {
                
                // 每趟比较次数
                for (int j = 0; j < length - i; j++)
                {
                    if (intList[j] > intList[j + 1])
                    {
                        int temp = intList[j + 1];
                        intList[j + 1] = intList[j];
                        intList[j] = temp;
                    }
                }
            }

            Console.WriteLine($"新数组元素如下:{string.Join(",", intList)}");
        }

        private static void Test3()
        {
            string documentType = "<!DOCTYPE html>\n";

            TagBuilder html = new TagBuilder("html");
            TagBuilder header = new TagBuilder("head");
            TagBuilder title = new TagBuilder("title");
            TagBuilder body = new TagBuilder("body");

            header.AppendInnerHtml(title.ToString());
            body.AppendInnerHtml("<h3>Test</h3>");

            html.AppendInnerHtml(header.ToString());
            html.AppendInnerHtml(body.ToString());

            Console.WriteLine(documentType + html.ToString());
        }

        private static void Test4()
        {
            var obj1 = new GetHashCodeTest()
            {
                X = 1,
                Y = 2
            };

            var obj2 = new GetHashCodeTest()
            {
                X = 1,
                Y = 2
            };

            Console.WriteLine(obj1.Equals(obj2));
        }

        private static void Test5()
        {
            var myClass1 = new List<MyClass1>
            {
                new MyClass1{ Id = 1, Name="张三" },
                new MyClass1{ Id = 2, Name="张三1" },
                new MyClass1{ Id = 3, Name="张三2" },
                new MyClass1{ Id = 4, Name="张三3" },
                new MyClass1{ Id = 5, Name="张三4" },
            };

            var myClass2 = new List<MyClass1>
            {
                new MyClass1{ Id = 1, Name="张三a" },
                new MyClass1{ Id = 2, Name="张三b" },
                new MyClass1{ Id = 3, Name="张三c" },
                new MyClass1{ Id = 4, Name="张三d" },
                new MyClass1{ Id = 5, Name="张三e" },
            };

            var result = myClass1.Join(myClass2,
                 o => o.Id,
                 p => p.Id,
                 (o, p) => new MyClass2
                 {
                     Guid = o.Id.ToString() + p.Id.ToString(),
                     Title = o.Name + p.Name
                 });

            Console.WriteLine(result.Count());
            //var myClass3 = new List<MyClass2>
            //{
            //    new MyClass2{ Guid = Guid.NewGuid().ToString(), Title = "张三" },
            //    new MyClass2{ Guid = Guid.NewGuid().ToString(), Title = "张三2" },
            //    new MyClass2{ Guid = Guid.NewGuid().ToString(), Title = "张三2" },
            //    new MyClass2{ Guid = Guid.NewGuid().ToString(), Title = "张三3" },
            //    new MyClass2{ Guid = Guid.NewGuid().ToString(), Title = "张三4" },
            //};
        }

        private static void Test6()
        {
            MyClass c1 = new MyClass()
            {
                Id = 1,
                StationName = "市二宫"
            };

            Console.WriteLine(c1.GetValueByPropName("StationName")); 
        }

        private static void Test7()
        {
            var s = new StaticClass("市二宫");
            Console.WriteLine(StaticClass.Result());
        }  

        /// <summary>
        /// 计算两个时间所跨的半年
        /// </summary>
        private static void Test8()
        {
            DateTime start = DateTime.Parse("2018-06");
            DateTime end = DateTime.Parse("2019-06");

            var year = end.Year - start.Year;
            var month = end.Month - start.Month;

            Console.WriteLine(year * 2 + (month / 6) + 1);

            Console.WriteLine(CalcDateTimeThroughHalfYear(start, end));
        }

        private static int CalcDateTimeThroughHalfYear(DateTime startMonth, DateTime endMonth)
        {
            int result = 0;
            int year = (endMonth.Year - startMonth.Year) + 1;

            for (int i = 0; i < year; i++)
            {
                if (i == 0)
                {
                    // 开始结束都在同一年则直接比较开始月和结束月
                    if (year == 1)
                    {
                        if (startMonth.Month < 7 && endMonth.Month > 6)
                        {
                            result += 2;
                        }
                        else
                        {
                            result += 1;
                        }
                    }
                    else// 反之则只比较开始月
                    {
                        if (startMonth.Month < 7)
                        {
                            result += 2;
                        }
                        else
                        {
                            result += 1;
                        }
                    }
                }
                else if (i == (year - 1))// 最后一项比较结束月
                {
                    if (endMonth.Month > 6)
                    {
                        result += 2;
                    }
                    else
                    {
                        result += 1;
                    }
                }
                else
                {
                    result += 2;
                }
            }
            return result;
        }

        private static void Test9()
        {
            uint num = 5;

            // 左移两位
            Console.WriteLine(num << 2);
        }

        private static void Test10()
        {
            NumberFormatInfo nfi = new CultureInfo(CultureInfo.CurrentCulture.Name, false).NumberFormat;
            nfi.PercentDecimalDigits = 1;
            Console.WriteLine(string.Format(nfi,"{0:P}", 0.95665));
        }

        public static double ExecuteFormula(string formula)
        {
            string S = ""; //后缀
            char[] Operators = new char[formula.Length];
            int Top = -1;
            for (int i = 0; i < formula.Length; i++)
            {
                char C = formula[i];
                switch (C)
                {
                    case ' ': //忽略空格
                        break;
                    case '+': //操作符
                    case '-':
                        while (Top >= 0) //栈不为空时
                        {
                            char c = Operators[Top--]; //pop Operator
                            if (c == '(')
                            {
                                Operators[++Top] = c; //push Operator
                                break;
                            }
                            else
                            {
                                S = S + c;
                            }
                        }
                        Operators[++Top] = C; //push Operator
                        S += " ";
                        break;
                    case '*': //忽略空格
                    case '/':
                        while (Top >= 0) //栈不为空时
                        {
                            char c = Operators[Top--]; //pop Operator
                            if (c == '(')
                            {
                                Operators[++Top] = c; //push Operator
                                break;
                            }
                            else
                            {
                                if (c == '+' || c == '-')
                                {
                                    Operators[++Top] = c; //push Operator
                                    break;
                                }
                                else
                                {
                                    S = S + c;
                                }
                            }
                        }
                        Operators[++Top] = C; //push Operator
                        S += " ";
                        break;
                    case '(':
                        Operators[++Top] = C;
                        S += " ";
                        break;
                    case ')':
                        while (Top >= 0) //栈不为空时
                        {
                            char c = Operators[Top--]; //pop Operator
                            if (c == '(')
                            {
                                break;
                            }
                            else
                            {
                                S = S + c;
                            }
                        }
                        S += " ";
                        break;
                    default:
                        S = S + C;
                        break;

                }
            }
            while (Top >= 0)
            {
                S = S + Operators[Top--]; //pop Operator
            }

            System.Console.WriteLine("后缀" + S); //后缀

            //后缀表达式计算
            double[] Operands = new double[S.Length];
            double x, y, v;
            Top = -1;
            string Operand = "";
            for (int i = 0; i < S.Length; i++)
            {
                char c = S[i];
                if ((c >= '0' && c <= '9') || c == '.')
                {
                    Operand += c;
                }

                if ((c == ' ' || i == S.Length - 1) && Operand != "") //Update
                {
                    Operands[++Top] = System.Convert.ToDouble(Operand); //push Operands
                    Operand = "";
                }

                if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    if ((Operand != ""))
                    {
                        Operands[++Top] = System.Convert.ToDouble(Operand); //push Operands
                        Operand = "";
                    }
                    y = Operands[Top--]; //pop 双目运算符的第二操作数 (后进先出)注意操作数顺序对除法的影响
                    x = Operands[Top--]; //pop 双目运算符的第一操作数
                    switch (c)
                    {
                        case '+':
                            v = x + y;
                            break;
                        case '-':
                            v = x - y;
                            break;
                        case '*':
                            v = x * y;
                            break;
                        case '/':
                            v = x / y; // 第一操作数 / 第二操作数 注意操作数顺序对除法的影响
                            break;
                        default:
                            v = 0;
                            break;
                    }
                    Operands[++Top] = v; //push 中间结果再次入栈
                }
            }
            v = Operands[Top--]; //pop 最终结果

            return v;
        }

        public static string GetTimePoint(List<DateTime> list)
        {
            list = list.OrderBy(p => p).ToList();

            var result = new List<DateTime[]>();
            if (list.Any())
            {
                DateTime[] dic = null;

                if(list.Count == 1)
                {
                    dic = new DateTime[] { list.First(), list.First() };
                }
                else
                {
                    // 比较当前时间和下一个时间是否相差1个小时
                    var currTime = list.First();
                    int i = 0;
                    foreach (var item in list)
                    {
                        if (i == 0)
                        {
                            dic = new DateTime[2];
                            dic[0] = currTime;
                        }
                        else if (item == list.Last())
                        {
                            dic[1] = item;
                            result.Add(dic);
                        }
                        else
                        {
                            if (item != currTime)
                            {
                                dic[1] = currTime.AddHours(-1);
                                result.Add(dic);

                                // 重置条件
                                currTime = list[i].AddHours(-1);
                                i = -1;
                            }
                        }
                        currTime = currTime.AddHours(1);
                        i++;
                    }
                }
            }
            return string.Join("\r\n", result.Select(p => string.Join("-", p.ToList().Select( o => o.ToString("yyyy年MM月dd日 HH:mm:ss")))));
        }

        public static void ExceptionTest()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                // 假如是英文...
                Console.WriteLine(ex.Message + "，堆栈信息:" + ex.StackTrace.Substring(ex.StackTrace.IndexOf("行号"), ex.StackTrace.Length - ex.StackTrace.IndexOf("行号")));
            }
        }
        
        #region Yield Test

        //public static IEnumerable<int> YieldTest()
        //{
        //    var counter = 0;
        //    int result = 1;

        //    while (++counter < 10)
        //    {
        //        result *= 2;
        //        yield return result;
        //    }
        //}
        #endregion
    }

    public static class TagBuilderExtension
    {
        public static void AppendInnerHtml(this TagBuilder tag, string html)
        {
            tag.InnerHtml += $"\n{html}";
        }
    }
}
