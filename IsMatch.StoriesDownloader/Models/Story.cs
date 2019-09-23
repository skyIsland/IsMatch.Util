using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.StoriesDownloader.Models
{
    public class Story
    {
        /// <summary> 名字 </summary>
        public string Title { get; set; }

        /// <summary> 地址前缀 </summary>
        public string UrlStart { get; set; }

        /// <summary> 首页地址 </summary>
        public string IndexUrl { get; set; }

        /// <summary> 描述 </summary>
        public string Description { get; set; }        

        /// <summary> 列表规则 </summary>
        public string ListRule { get; set; }

        /// <summary> 内容规则 </summary>
        public string DetailRule { get; set; }

    }
}
