using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Models
{
    /// <summary>
    /// 带分页的返回结果
    /// </summary>
    public class PageResult<T> : Prompt<T>
    {
        #region 属性

        /// <summary> 页码 </summary>
        public int PageIndex { get; set; }

        /// <summary> 每页数量 </summary>
        public int PageSize { get; set; }

        /// <summary> 总数 </summary>
        public long TotalCount { get; set; }

        /// <summary> 页数 </summary>
        public long PageCount { get; set; }

        #endregion

        #region 方法
        /// <summary>从后端分页器转换</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PageResult<T> FromPager(NewLife.Data.PageParameter p)
        {
            return new PageResult<T> { PageIndex = p.PageIndex, PageSize = p.PageSize, TotalCount = p.TotalCount, PageCount = p.PageCount };
        }

        #endregion
    }

    /// <summary>带分页的Ajax返回结果</summary>
    public class PageResult : PageResult<object> { }
}
