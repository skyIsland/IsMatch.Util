using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Spider.Txt
{
    public class Setting
    {
        public int RuleId { get; set; }

        public string StoryId { get; set; }
    }

    public class Rule
    {
        public int Id { get; set; }

        public string TxtIndexUrl { get; set; }

        public string UrlStart { get; set; }

        public string ListRule { get; set; } = "#list>dl>dd>a";

        public string DetailRule { get; set; } = "#content";

        public string CharCode { get; set; } = "UTF8";
    }
}
