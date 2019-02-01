using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsMatch.Blog.Controllers
{
    public class PostController : BaseController
    {
        [Route("Post/{id}")]
        public ActionResult Post()
        {
            return View();
        }
    }
}