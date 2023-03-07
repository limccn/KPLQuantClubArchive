using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.WebPages;

namespace KaiPanLaWeb.Filters
{

    public class WeixinMiniAppOnlyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {

            // 拆解refer
            string qAppId = "";
            // 拆解refer获得appid
            if (context.Request.Headers.Referrer != null)
            {
                string strRef = context.Request.Headers.Referrer.ToString().ToLower();
                Uri uri = new Uri(strRef);
                if (uri.Segments.Length > 1)
                {
                    qAppId = uri.Segments[1].Replace("/", "");
                    if (qAppId == null || qAppId.IsEmpty())
                    {
                        context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "empty weixin appid");
                    }
                }
                else
                {
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "empty weixin appid");

                }
            }
            string wxspSecret = Common.GetAppSettingByKey(qAppId);
            //判断app有效
            if (wxspSecret == null || wxspSecret.IsEmpty())
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "weixin appid not registered");
            }

            base.OnActionExecuting(context);
        }
    }

    public class WeixinMiniProgramFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {

            // 拆解refer
            if (context.Request.Headers.Referrer != null)
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