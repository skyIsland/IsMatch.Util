using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.WordHelper
{
    class Program
    {
        static object _unite = WdUnits.wdStory;
        static object _missVal = Missing.Value;
        //全选内容
        static object _repAll = WdReplace.wdReplaceAll;

        static void Main(string[] args)
        {
            // 文字替换

            // TODO图

            // 表格

            try
            {
                string targetPath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Template.docx");

                Test1(targetPath);

                Console.WriteLine("OK...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常，异常信息:{ex.Message}");
            }            
            Console.ReadKey();
        }

        private static void Test1(string tagWordPath)
        {
            // Word应用程序变量
            Application wordApp = new Application { Visible = false };

            // Word文档变量
            Document wordDoc = wordApp.Documents.Open(tagWordPath);

            try
            {
               
                // 替换文字
                var dic = new Dictionary<string, string>()
                {
                    ["ReplaceTxt"] = $"在 { DateTime.Now.ToLongDateString() } 替换的哟..."
                };

                ReplaceContent(dic, wordApp);

                var table = wordDoc.Tables[1];
                int rowIndex = 2;
                var list = GetList();

                foreach (var item in list)
                {
                    int cellIndex = 1;

                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Title;
                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Content;
                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Remark;
                }
                table.Cell(rowIndex, 1).Range.Rows.Delete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wordDoc.Close();
                wordApp.Quit();
            }
           
        }

        /// <summary>
        /// 内容替换
        /// </summary>
        /// <param name="dataDic"></param>
        /// <param name="wordApp"></param>
        public static void ReplaceContent(Dictionary<string, string> dataDic, _Application wordApp)
        {
            foreach (var data in dataDic)
            {
                    //光标定位到文档开头
                    wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                    wordApp.Selection.Find.ClearFormatting();
                    wordApp.Selection.Find.Text = data.Key.Trim();
                    wordApp.Selection.Find.MatchCase = true;//是否区分大小写
                    wordApp.Selection.Find.Replacement.ClearFormatting();
                    wordApp.Selection.Find.Replacement.Text = data.Value;
                    //执行全部替换
                    wordApp.Selection.Find.Execute(ref _missVal, ref _missVal, ref _missVal, ref _missVal,
                        ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _repAll,
                        ref _missVal, ref _missVal, ref _missVal, ref _missVal);
            }
        }

        public static List<dynamic> GetList()
        {
            var list = new List<dynamic>();

            for (int i = 0; i < 5; i++)
            {
                dynamic o = new System.Dynamic.ExpandoObject();
                o.Title = "" + i.ToString();
                o.Content = "" + i.ToString();
                o.Remark = "" + i.ToString();

                list.Add(o);
            }

            return list;
        }
    }
}
