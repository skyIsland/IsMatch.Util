using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsMatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IsMatch.Blog.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Prepare();
            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Prepare();
            return base.OnActionExecutionAsync(context, next);
        }

        public virtual void Prepare()
        {
            ViewBag.Position = "home";

            ViewBag.Catalogs = Catalogs.FindAll().OrderBy( p => p.ID);
        }

        /// <summary>
        /// 友情提示页（待添加一个实体）
        /// </summary>
        /// <returns></returns>
        public virtual IActionResult Prompt()
        {
            return View("Error");
        }
    }
}