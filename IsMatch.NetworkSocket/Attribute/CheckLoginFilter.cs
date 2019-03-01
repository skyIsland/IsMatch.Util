using NetworkSocket.Core;
using NetworkSocket.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsMatch.NetworkSocket.Attribute
{
    public class CheckLoginFilter : HttpFilterAttribute
    {
        protected override  void OnExecuted(ActionContext filterContext)
        {
            bool result = true;

            // 简单判断Token
            var action = filterContext.Action;
            string apiName = action.ApiName;

            if (!apiName.Contains("Token"))
            {
                var token = action.RouteDatas["token"];
                if(token == null || string.IsNullOrWhiteSpace(token))
                {
                    result = false;
                }
            }
            if (!result) filterContext.Result = new JsonResult(new { Result = false, Token = "Token过期" });
        }
    }
}
