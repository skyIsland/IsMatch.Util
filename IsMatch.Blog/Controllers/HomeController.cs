using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IsMatch.Blog.Models;
using IsMatch.Models;
using NewLife.Data;

namespace IsMatch.Blog.Controllers
{
    public class HomeController : BaseController
    {
        [Route("{p:int?}")]
        public IActionResult Index(int p = 1)
        {
            return View(Posts.PageView(null, new PageParameter { PageIndex = p, PageSize = 5 }));
        }

        [Route("Catalog/{id}/{p:int?}")]
        public IActionResult Catalog(string id, int p = 1)
        {
            var catalog = Catalogs.FindByUrl(id);

            if (catalog == null)
            {
                return Prompt();
            }

            ViewBag.Position = catalog.Url;

            var list = Posts.PageView(id, new PageParameter { PageIndex = p, PageSize = 5 });

            // 只有一篇文章时直接跳转到详情页
            if(list.Count == 1)
            {
                return View("Post", list.FirstOrDefault());
            }

            return View("Index", Posts.PageView(id, new PageParameter { PageIndex = p, PageSize = 5 }));
        }
    }
}
