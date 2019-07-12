using System;
using System.Collections.Generic;
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Test1();

            //Test2();

            //Test3();

            //Test4();

            //Test5();

            //Test6();

            //Test7();

            //Test8();

            Test9();

            Console.ReadKey();
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

    }

    public static class TagBuilderExtension
    {
        public static void AppendInnerHtml(this TagBuilder tag, string html)
        {
            tag.InnerHtml += $"\n{html}";
        }
    }
}
