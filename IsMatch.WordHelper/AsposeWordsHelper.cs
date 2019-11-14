using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsMatch.WordHelper
{
    public class AsposeWordsHelper
    {
        /// <summary>
        /// 书签替换，字典和word书签必须一致,如果字典包含word模板不存在的书签，这些不存在的字典将在word的最后一个书签进行叠加显示
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="asTemplates">字典</param>
        /// <param name="tableData">表格</param>
        /// <param name="replaceType">bookmark:书签替换,placeholder:占位符替换</param>
        public static void FillWordData(string wordFile, string pdfFile, List<ASTemplate> asTemplates, List<TempClass> tableData, ReplaceTypeEnum replaceType = ReplaceTypeEnum.bookmark)
        {
            Document doc = new Document(wordFile);
            DocumentBuilder builder = new DocumentBuilder(doc);   //操作word

            
            if (replaceType == ReplaceTypeEnum.bookmark)
            {
                //书签替换
                asTemplates.ForEach(a =>
                {
                    builder.MoveToBookmark(a.Key);  //将光标移入书签的位置
                    builder.Write(a.Value);   //填充值
                });
            }
            else
            {
                //占位符替换
                asTemplates.ForEach(a =>
                {
                    //doc.Range.Replace(a.Key, a.Value, false, false);
                    doc.Range.Replace(a.Key, a.Value);
                });
            }
            
            //获取表格节点集合
            NodeCollection tables = doc.GetChildNodes(NodeType.Table, true);
            //填充表格数据
            for (int i = 0; i < tables.Count(); i++)
            {
                AddTableRow(tables[i] as Table, tableData, doc, builder);
            }

            doc.Save(wordFile);//保存word
            //doc.Save(pdfFile, SaveFormat.Pdf);//保存pdf
        }

        //private static void AddTableRow(Table table, List<TempClass> data, Document doc, DocumentBuilder builder)
        //{
        //    if (data.GetType() == typeof(List<TempClass>))
        //    {
        //        AddTableRow(table, (data as List<TempClass>), doc, builder);
        //    }
        //}

        private static void AddTableRow(Table table, List<TempClass> dataList, Document doc, DocumentBuilder builder)
        {
            var cellLength = table.FirstRow.Cells.Count;

            if (table.Rows.Count > 1)
            {
                table.LastRow.Remove();
            }

            //builder.MoveTo(table);
            builder.MoveToBookmark("table");
            for (int i = 0; i < dataList.Count; i++)
            {
                for (int j = 0; j < cellLength; j++)
                {
                    builder.InsertCell();// 添加一个单元格                    
                    builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                    builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    //builder.CellFormat.Width = widthList[j];
                    builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐   
                    
                    if(j == 0)
                    {
                        builder.Write(dataList[i].Province);
                    }
                    else if(j == 1)
                    {
                        builder.Write(dataList[i].City);
                    }
                    else if(j == 2)
                    {
                        builder.Write(dataList[i].DustValue.ToString());
                    }
                }                

                builder.EndRow();
            }          

            //foreach (var item in dataList)
            //{               
            //    var row = CreateRow(3, new[]
            //    {
            //        item.Province,
            //        item.City,
            //        item.DustValue.ToString()
            //    }, doc);

            //    table.AppendChild(row);
            //}
        }

        public static Row CreateRow(int columnCount, string[] columnValues, Document doc)
        {
            Row r = new Row(doc);
            for (int i = 0; i < columnCount; i++)
            {
                if (columnValues.Length > i)
                {
                    var cell = CreateCell(columnValues[i], doc);
                    r.Cells.Add(cell);
                }
                else
                {
                    var cell = CreateCell("", doc);
                    r.Cells.Add(cell);
                }

            }
            return r;
        }
        public static Cell CreateCell(string value, Document doc)
        {
            Cell c = new Cell(doc);
            Paragraph p = new Paragraph(doc);
            p.AppendChild(new Run(doc, value));
            c.AppendChild(p);
            return c;
        }
    }

    public class ASTemplate
    {
        public string Key { set; get; }
        public string Value { set; get; }
    }

    public enum ReplaceTypeEnum
    {
        /// <summary>
        /// 书签替换
        /// </summary>
        bookmark = 1,

        /// <summary>
        /// 占位符替换
        /// </summary>
        placeholder = 2
    }

    public class TempClass
    {
        public string Province { get; set; }

        public string City { get; set; }

        public decimal DustValue { get; set; }
    }
}
