using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsMatch.Bill.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IsMatch.Bill.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected EfDbContext _context;
        public BaseController(EfDbContext context)
        {
            this._context = context;
        }       
    }
}