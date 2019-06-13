using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IsMatch.NfxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();

            //Test2();

            Test3();

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
    }

    public static class TagBuilderExtension
    {
        public static void AppendInnerHtml(this TagBuilder tag, string html)
        {
            tag.InnerHtml += $"\n{html}";
        }
    }
}
