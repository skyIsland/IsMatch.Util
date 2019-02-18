using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsMatch.Bill.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IsMatch.Bill.WebApi.Controllers
{
    [Route("api/Bill")]
    public class BillManagerController : BaseController
    {
        public BillManagerController(EfDbContext context) : base(context)
        {

        }

        [HttpGet, Route("GetList")]
        public IList<BillInfo> GetList(int pageIndex = 1, int pageSize = 10)
        {
            return _context.BillInfo.Where(p => p.IsDelete == 0).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        [HttpGet, Route("Edit")]
        public BillInfo Edit(int id)
        {
            return _context.BillInfo.FirstOrDefault(p => p.Id == id && p.IsDelete == 0);
        }

        [HttpPost, Route("Save")]
        public int Save(BillInfo entity)
        {
            if (entity.Id == 0) entity.AddTime = DateTime.Now;

            entity.EditTime = DateTime.Now;
            _context.Add(entity);
            return _context.SaveChanges();
        }

        [HttpDelete, Route("Delete")]
        public int Delete(int id)
        {
            var billInfo = _context.BillInfo.FirstOrDefault(p => p.Id == id);
            if (billInfo == null)
            {
                return 0;
            }
            billInfo.IsDelete = 1;
            return Save(billInfo);
        }
    }
}