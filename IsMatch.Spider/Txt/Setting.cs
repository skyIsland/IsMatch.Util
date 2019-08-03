using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Spider.Txt
{
    public class Setting
    {
        public string TxtIndexUrl { get; set; }

        public string UrlStart { get; set; }

        public string ListRule { get; set; } = "#list>dl>dd>a";

        public string DetailRule { get; set; } = "#content";
    }
}
