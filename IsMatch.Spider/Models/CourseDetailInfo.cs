using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Spider.Models
{
    public class CourseDetailInfo
    {
        public string courseId { get; set; }
        public string name { get; set; }
        public Treedata[] treeData { get; set; }
    }

    public class Treedata
    {
        public string nodeId { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public Treedata[] children { get; set; }
        public bool selected { get; set; }
        public bool disable { get; set; }
        public bool expanded { get; set; }
    }


    public class CourseDetail
    {
        public string _id { get; set; }
        public string rootVirtualPath { get; set; }
        public string originalFileDownloadURL { get; set; }
        public string sourceCodeDownloadURL { get; set; }
        public string resourceGuid { get; set; }
        public bool isWideScreen { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int difficultyLevel { get; set; }
        public int viewCount { get; set; }
        public int voteCount { get; set; }
        public object[] comments { get; set; }
    }

}
