using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsMatch.Blog.Controllers
{
    public class HomeController : BaseController
    {
        [Route("{p:int?}")]
        public ActionResult Index(int p = 1)
        {
            var posts = _db.T($"SELECT * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY ID DESC) rowId FROM dbo.Posts) AS b WHERE b.rowId BETWEEN -1 * {p} AND {p * 5}").ExecuteDataTable();
            return View(posts);
        }

        [Route("catalog/{id}/{p:int?}")]
        public ActionResult Catalog(string id, int p = 1)
        {
            // 查找分类
            var catalog = _db.T("SELECT TOP 1 * FROM dbo.Catalogs WHERE Url = '" + id + "'").ExecuteFirstRow();
            if (catalog == null)
            {
                return NotFound();
            }
            ViewBag.Title = catalog["Title"];

            // 页面直接转跳
            if (catalog["IsPage"].ToString() == "1")
            {
                return Redirect(catalog["Url"].ToString());
            }        
            
            // 下一页地址
            ViewBag.PageUrl = "" + id + "/" + (p + 1);

            // rows number分页 BETWEEN 当前页数 -1 * 条数 and 页数 * 条数     
            var posts = _db.T($"SELECT * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY ID DESC) rowId FROM dbo.Posts WHERE CatalogId = {catalog["ID"]}) AS b WHERE b.rowId BETWEEN -1 * {p} AND {p * 5}").ExecuteDataTable();
            return View("Index", posts);
        }
    }
}