using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsMatch.Models;
using Microsoft.AspNetCore.Mvc;

namespace IsMatch.Blog.Controllers
{
    public class PostController : BaseController
    {
        [Route("Post/{id}")]
        public IActionResult Post(string id)
        {
            var post = Posts.FindByUrl(id);
            if (post == null)
            {
                return Prompt();
            }

            ViewBag.Title = post.Title;
            ViewBag.Position = post.CatalogsTitle.IsNullOrEmpty() ? "home" : post.CatalogsTitle;
            return View(post);
        }
    }
}