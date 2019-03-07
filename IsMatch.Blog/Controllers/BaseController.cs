using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IsMatch.Blog.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public virtual void Prepare()
        {
           
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}