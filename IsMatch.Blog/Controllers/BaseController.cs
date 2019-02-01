using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ivony.Data;
using Ivony.Data.SqlClient;

namespace IsMatch.Blog.Controllers
{
    public class BaseController : Controller
    {
        protected SqlDbExecutor _db;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _db = SqlServer.FromConfiguration("Conn");
            Prepare();            
            base.OnActionExecuting(filterContext);
        }

        public virtual void Prepare()
        {
            ViewBag.Catalogs = _db.T("SELECT * FROM Catalogs").ExecuteDataTable();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}