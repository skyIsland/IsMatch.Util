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

            // 图
            try
            {
                string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Template1.docx");
                string destPath = Path.Combine(Directory.GetCurrentDirectory(), "Template",
                Guid.NewGuid().ToString() + ".docx");
                File.Copy(sourcePath, destPath);

                //Test1(destPath);

                AsposeTest(destPath);

                Console.WriteLine("OK...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常，异常信息:{ex.Message}");
            }            
            Console.ReadKey();
        }


        #region Test1

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
                    ["ReplaceTxt"] = $"在 { DateTime.Now.ToLongDateString() } 替换的哟...μg/m3 μg/m3μg/m3μg/m3"
                };

                ReplaceContent(dic, wordApp);

                //wordDoc.ActiveWindow.Visible = true;
                //var r = wordDoc.Range();
                //r.Font.Name = "华文行楷 常规";
                //r.Select();
                //wordDoc.Save();

                #region
                //μg/m³
                wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                wordApp.Selection.Find.Text = "μg/m³";
                wordApp.Selection.Find.MatchCase = true;//是否区分大小写
                wordApp.Selection.Find.Font.NameOther = "Times New Roman";

                wordApp.Selection.Find.Execute(ref _missVal, ref _missVal, ref _missVal, ref _missVal,
                        ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _missVal, ref _repAll,
                        ref _missVal, ref _missVal, ref _missVal, ref _missVal);

                //wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                //wordApp.Selection.Find.Text = "μ";
                //wordApp.Selection.Find.MatchCase = true;//是否区分大小写
                //wordApp.Selection.Find.Font.NameOther = "Times New Roman";

                //wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                //wordApp.Selection.Find.Text = "³";
                //wordApp.Selection.Find.MatchCase = true;//是否区分大小写
                //wordApp.Selection.Find.Font.NameOther = "Times New Roman";
                #endregion

                #region
                ////光标定位到文档开头
                //wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                //wordApp.Selection.Find.ClearFormatting();
                //wordApp.Selection.Find.Font.Bold = 0;
                //wordApp.Selection.Find.Font.Italic = 0;
                //wordApp.Selection.Find.Text = "μ";
                //wordApp.Selection.Font.Name = "Times New Roman";
                #endregion

                #region 更改字体
                ////光标定位到文档开头
                //wordApp.Selection.HomeKey(ref _unite, ref _missVal);
                //wordApp.Selection.Find.ClearFormatting();
                //wordApp.Selection.Find.Font.Bold = 0;
                //wordApp.Selection.Find.Font.Italic = 0;
                //wordApp.Selection.Find.Text = "替换";
                //var r = wordApp.Selection.Range;

                //r.Font.Name = "华文琥珀 常规";
                ////wordApp.Selection.Find.Font.Name = "华文琥珀 常规";

                ////var r = wordDoc.Range();
                //r.Select();
                //wordDoc.Save();

                //r.Font.Size = 20;
                //r.Font.Name = "华文琥珀 常规";
                #endregion

                var table = wordDoc.Tables[1];
                int rowIndex = 2;
                var list = GetList();

                foreach (var item in list)
                {
                    int cellIndex = 1;

                    table.Rows.Add(ref _missVal);

                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Title;
                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Content;
                    table.Cell(rowIndex, cellIndex++).Range.Text = item.Remark;

                    rowIndex++;
                }

                table.Cell(rowIndex, 1).Range.Rows.Delete();

                wordDoc.Save();
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

            for (int i = 1; i < 6; i++)
            {
                dynamic o = new System.Dynamic.ExpandoObject();
                o.Title = "标题" + i.ToString();
                o.Content = "内容" + i.ToString();
                o.Remark = "备注" + i.ToString();

                list.Add(o);
            }

            return list;
        }

        #endregion

        #region Aspose.Word

        public static void AsposeTest(string filePath)
        {
            var asTemplate = new List<ASTemplate> { new ASTemplate { Key = "#ReplaceTxt#", Value = "我是替换之后的文本。" } };
            var tabData = new List<TempClass>()
            {
                new TempClass
                {
                    Province = "山西省",
                    City ="太原市",
                    DustValue = 5.7M
                },
                new TempClass
                {
                    Province = "河北省",
                    City ="石家庄市",
                    DustValue = 5.71M
                }
            };

            AsposeWordsHelper.FillWordData(filePath, "", asTemplate, tabData, ReplaceTypeEnum.placeholder);
        }

        #endregion
    }
}
