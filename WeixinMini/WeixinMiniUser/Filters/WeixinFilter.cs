using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WeixinMiniUser.Filters
{

    public class WeixinMiniProgramFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {

            // 拆解refer
            if(context.Request.Headers.Referrer != null)
            {
                string strRef = context.Request.Headers.Referrer.ToString();
            }

            base.OnActionExecuting(context);
        }
    }

    public class WeixinOnlyFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            var verifyResult = context.Request.Headers.Referrer != null &&  //必须有Referrer头
                               context.Request.Headers.Referrer.ToString().IndexOf("servicewechat") >= 0; //包含来自微信的servicewechat申明

            if (!verifyResult)
            {
                //如果验证不通过，则返回401错误
  
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "use weixin mini program to access this api");

            }

            base.OnAuthorization(context);
        }
    }
}