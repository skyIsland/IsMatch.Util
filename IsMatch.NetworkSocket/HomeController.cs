using IsMatch.NetworkSocket.Attribute;
using NetworkSocket.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.NetworkSocket
{
    [CheckLoginFilter]
    public class HomeController : HttpController
    {
        [HttpGet]
        public dynamic GetUsers(string name)
        {
            var data = new List<dynamic>
            {
              new { Name = "list1" },
              new { Name = "list2" }
            };
            return new List<dynamic> { new { Result = true, Data = data } };
        }
    }
}
