using IsMatch.Models;
using IsMatch.NetworkSocket.Attribute;
using NetworkSocket.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.NetworkSocket
{
    //[CheckLoginFilter]
    public class HomeController : HttpController
    {
        [HttpGet]
        public Prompt<object> GetList(NewLife.Data.PageParameter p, string token = null)
        {
            if (token.IsNullOrEmpty())
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            if(token != "123456")
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            var list = Bill.Search(null, p);
            var result = PageResult.FromPager(p);
            result.Data = list;

            return result;
        }

        [HttpGet]
        public Prompt<object> GetById(int id, string token = null)
        {
            if (token.IsNullOrEmpty())
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            if (token != "123456")
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            var info = Bill.FindByID(id);
            if(info == null)
            {
                return new Prompt { Status = 0, Message = "该记录不存在！" };
            }
            return new Prompt { Status = 0, Data = info };
        }


        [HttpPost]
        public Prompt<object> Save(Bill model, string token = null)
        {
            if (token.IsNullOrEmpty())
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            if (token != "123456")
            {
                return new Prompt { Status = 0, Message = "Token不存在" };
            }

            int result = model.Save();
            if(result > 0)
            {
                return new Prompt();
            }

            return new Prompt { Status = 0, Message = "保存失败！" };
        }
    }
}
