using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.StoriesDownloader.Provider
{
    public interface IStoriesProvider
    {
        string ProviderName { get; set; }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<string> SearchStories(string keyword, int pageNo, int pageSize);


    }
}
