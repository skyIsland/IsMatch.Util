using IsMatch.Spider.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IsMatch.Spider
{
    public class CourseSource
    {
        private string ListUrl = "http://www.jinxuliang.com/course2/api/courseService";
        private string DetailUrl = "http://www.jinxuliang.com/course2/api/courseService/tree/{0}";
        public void Work()
        {
            List<Course> courseList = GetList();
            Console.WriteLine($"共检测到{courseList.Count}个课程。");
            Console.WriteLine("\n开始下载相关资源");
            GetDetail(courseList);
        }

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

        private void GetDetail(List<Course> courseList)
        {
            int count = courseList.Count;
            System.Threading.Tasks.Parallel.For(0, count, (i, state) =>
            {
                using(var wc = new WebClient())
                {
                    Console.WriteLine("\n" + courseList[i].name + "开始下载...");
                    var html = wc.DownloadString(string.Format(DetailUrl, courseList[i]._id));
                    ; Console.WriteLine("\n" + courseList[i].name + "\n返回JSON" + html);
                }
            });
        }
    }
}
