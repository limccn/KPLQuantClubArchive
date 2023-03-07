using WeixinMiniUser.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.WebPages;
using WeixinMiniUser.Filters;

namespace WeixinMiniUser.Models
{
    public class SubscribeController : ApiController
    {
        // GET api/<controller>
        [WeixinOnlyFilter]
        public Message<UserSubscribe> Get(string oid)
        {
            Message<UserSubscribe> result = new Message<UserSubscribe>();
            string qAppId = "";
            // 拆解refer获得appid
            if (ActionContext.Request.Headers.Referrer != null)
            {
                string strRef = ActionContext.Request.Headers.Referrer.ToString().ToLower();
                Uri uri = new Uri(strRef);
                if (uri.Segments.Length > 1)
                {
                    qAppId = uri.Segments[1].Replace("/", "");
                    if (qAppId == null || qAppId.IsEmpty())
                    {
                        result.code = 400;
                        result.message = "empty wx app id";
                        result.detail = null;
                        return result;
                    }
                }
                else{
                    result.code = 400;
                    result.message = "empty wx app id";
                    result.detail = null;
                    return result;
                }
            }
            string wxspSecret = Common.GetAppSettingByKey(qAppId);
            if (wxspSecret == null || wxspSecret.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid wx appid";
                result.detail = null;
                return result;
            }

            string qOid = oid;
            if (oid == null || oid.IsEmpty())
            {
                result.code = 400;
                result.message = "empty open id";
                result.detail = null;
                return result;
            }

            Regex rgx = new Regex("[\\w\\-_]{20,40}");

            if (!rgx.IsMatch(oid))
            {
                result.code = 400;
                result.message = "invalid oid";
                result.detail = null;
                return result;
            }

            UserInfoDao userdao = new UserInfoDao();
            SubscribeDao subdao = new SubscribeDao();
            
            WXUserInfo wxuser = userdao.Query(qOid,qAppId);

            if(wxuser == null)
            {
                result.code = 404;
                result.message = "user is not valid";
                result.detail = null;
                return result;
            }

            UserSubscribe param = new UserSubscribe();
            param.open_id = wxuser.openId;
            param.union_id = wxuser.unionId;

            UserSubscribe sub = subdao.QueryOrCreate(param,qAppId);

            if(sub == null)
            {
                result.code = 400;
                result.message = "write to database error";
                result.detail = null;
            }
            else{
                result.code = 200;
                result.message = "success";
                result.detail = sub;
            }

            return result;
        }
    }
}
