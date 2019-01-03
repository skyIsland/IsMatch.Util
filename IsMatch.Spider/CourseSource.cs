using IsMatch.Spider.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace IsMatch.Spider
{
    public class CourseSource
    {
        private string ListUrl = "http://www.jinxuliang.com/course2/api/courseService";
        private string DetailInfoUrl = "http://www.jinxuliang.com/course2/api/courseService/tree/{0}";
        private string DetailUrl = "http://www.jinxuliang.com/course2/api/pptService/{0}";
        private string BaseDir = @"E:\Study\CrawlerResult\JinXuLiang";        

        public void Work()
        {
            List<Course> courseList = GetList();
            Console.WriteLine($"共检测到{courseList.Count}个课程。");
            Console.WriteLine("\n开始下载相关资源");
            GetDetail(courseList);
        }


        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        private List<Course> GetList()
        {
            List<Course> listCourse = new List<Course>();
            using (var wc = new WebClient())
            {
                string html = wc.DownloadString(ListUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    listCourse = JsonConvert.DeserializeObject<List<Course>>(html);
                }
            }
            return listCourse;
        }

        /// <summary>
        /// 获取课程详情
        /// </summary>
        /// <param name="courseList"></param>
        private void GetDetail(List<Course> courseList)
        {
            int count = courseList.Count;
            //System.Threading.Tasks.Parallel.For(0, count, (i, state) =>
            //{
            //    using (var wc = new WebClient())
            //    {
            //        Console.WriteLine("\n" + courseList[i].name + "开始分析...");
            //        string jsonString = wc.DownloadString(string.Format(DetailInfoUrl, courseList[i]._id));
            //        CourseDetailInfo detail = JsonConvert.DeserializeObject<CourseDetailInfo>(jsonString);
            //        if (detail != null && detail.treeData.Length > 0)
            //        {
            //            CategoryNameTarget = "";
            //            ScanChildren(detail.treeData[0]);
            //        }
            //    }

            //    Console.WriteLine(courseList[i].name + "download complete...");
            //});

            foreach (var item in courseList)
            {
                using (var wc = new WebClient())
                {
                    Console.WriteLine("\n" + item.name + "开始分析...");
                    string jsonString = wc.DownloadString(string.Format(DetailInfoUrl, item._id));
                    CourseDetailInfo detail = JsonConvert.DeserializeObject<CourseDetailInfo>(jsonString);
                    if (detail != null && detail.treeData.Length > 0)
                    {
                        ScanChildren(detail.treeData[0]);
                    }
                }

                Console.WriteLine(item.name + "download complete...");
            }
        }

        // 记录当前路径
        private string CategoryNameTarget = "";

        /// <summary>
        /// 提取课程id
        /// </summary>
        private void ScanChildren(Treedata children)
        {
            if (children.children.Length > 0)
            {
                // 记录当前路径
                if(!CategoryNameTarget.Contains(children.name)) CategoryNameTarget += "\\" + children.name;

                // 遍历每一次子目录
                foreach (var childrenChild in children.children)
                {
                    // 记录当前路径
                    if (!CategoryNameTarget.Contains(childrenChild.name)) CategoryNameTarget += "\\" + childrenChild.name;
                    ScanChildren(childrenChild);
                }

                // 回退到上一路径
                CategoryNameTarget = CategoryNameTarget.Replace("\\" + children.name, "");
            }
            else
            {
                DownloadPowerPointAndCode(children.nodeId, CategoryNameTarget);

                // 回退到上一路径
                CategoryNameTarget = CategoryNameTarget.Replace("\\" + children.name, "");
            }
        }

        /// <summary>
        /// 下载PPT和源码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        private void DownloadPowerPointAndCode(string id, string categoryName)
        {
            try
            {
                string directoryName = BaseDir + categoryName;
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                using (var wc = new WebClient())
                {
                    Console.WriteLine("\n" + id + "开始下载...");
                    string jsonString = wc.DownloadString(string.Format(DetailUrl, id));
                    CourseDetail detail = JsonConvert.DeserializeObject<CourseDetail>(jsonString);

                    if (!string.IsNullOrWhiteSpace(detail.sourceCodeDownloadURL))
                    {
                        var name = "\\" + Path.GetFileName("http://www.jinxuliang.com" + detail.sourceCodeDownloadURL);
                        wc.DownloadFile("http://www.jinxuliang.com" + detail.sourceCodeDownloadURL, directoryName + name);
                    }

                    if (!string.IsNullOrWhiteSpace(detail.originalFileDownloadURL))
                    {
                        var name = "\\" + Path.GetFileName("http://www.jinxuliang.com" + detail.originalFileDownloadURL);
                        wc.DownloadFile("http://www.jinxuliang.com" + detail.originalFileDownloadURL, directoryName + name);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("下载PPT和源码发生错误，原因：" + ex.Message);
            }
        }

        private string ModFilePath(string filePath)
        {
         
            StringBuilder rBuilder = new StringBuilder(filePath);
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), string.Empty);
            }

            rBuilder = new StringBuilder(rBuilder.ToString());
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), string.Empty);

            }

            return rBuilder.ToString();
        }
    }
}
