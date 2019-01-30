using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IsMatch.Bill.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}