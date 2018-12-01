using System;

namespace IsMatch.CnArticleSubscribe.Models
{
    public class Article
    {
        /// <summary> 排序</summary>
        public int No { get; set; }

        /// <summary> 标题</summary>
        public string Title { get; set; }

        /// <summary> 文章url</summary>
        public string Url { get; set; }

        /// <summary> 摘要</summary>
        public string Summary { get; set; }

        /// <summary> 作者</summary>
        public string Author { get; set; }

        /// <summary> 发布时间</summary>
        public DateTime PublishTime { get; set; }

        /// <summary> 评论量</summary>
        public int CommentCount { get; set; }

        /// <summary> 浏览量</summary>
        public int ViewCount { get; set; }
    }
}
