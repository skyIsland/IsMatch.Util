using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Spider.Models
{
    public class Course
    {            
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool isTopModule { get; set; }
        public string resourceGuid { get; set; }
        public int voteCount { get; set; }
    }
}
