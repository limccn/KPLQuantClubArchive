using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Filters;
using KaiPanLaWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.WebPages;

namespace KaiPanLaWeb.Controllers
{
    public class UserInfoController : ApiController
    {

        [WeixinOnlyFilter]
        public Message<UserInfo> Get(string cd)
        {

            Message<UserInfo> result = new Message<UserInfo>();
            UserInfo respo = new UserInfo();

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
                else
                {
                    result.code = 400;
                    result.message = "empty wx app id";
                    result.detail = null;
                    return result;
                }
            }
            //登录凭证不能为空
            if (cd == null || cd.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid code";
                result.detail = respo;
                return result;
            }

            Regex rgx = new Regex("[\\w\\-_]{20,50}");

            if (!rgx.IsMatch(cd))
            {
                result.code = 400;
                result.message = "invalid cd";
                result.detail = null;
                return result;
            }

            //小程序唯一标识   (在微信小程序管理后台获取)
            string wxspAppid = qAppId;
            //小程序的 app secret (在微信小程序管理后台获取)
            string wxspSecret = Common.GetAppSettingByKey(qAppId);

            //判断app有效
            if (wxspSecret == null || wxspSecret.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid wx appid, appsecet is not found";
                result.detail = respo;
                return result;
            }
            //授权（必填）
            string grant_type = "authorization_code";
            string wxSessionUrl = Common.GetAppSettingByKey("WXMiniProgramSessionAPIUrl");

            HttpClient client = new HttpClient();
            //1、向微信服务器 使用登录凭证 code 获取 session_key 和 openid
            //请求参数
            string url_params = "appid=" + wxspAppid +
                                "&secret=" + wxspSecret +
                                "&js_code=" + cd +
                                "&grant_type=" + grant_type;
            //发送请求
            string url = wxSessionUrl + "?" + url_params;

            try
            {
                using (HttpWebResponse resp = KaiPanLaCommon.HttpHelper.CreateGetHttpResponse(url))
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        using (System.IO.Stream stream = resp.GetResponseStream())
                        {
                            StreamReader sr = new StreamReader(stream);

                            string temp = sr.ReadToEnd();
                            JObject jo = (JObject)JsonConvert.DeserializeObject(temp);
                            WXSessionKey sessionKeyObj = JsonConvert.DeserializeObject<WXSessionKey>(jo.ToString());
                            if (sessionKeyObj.errcode > 0)
                            {
                                //失败
                                result.code = 400;
                                result.message = "get openid from weixin error, detail " + sessionKeyObj.errmsg;
                                result.detail = respo;
                            }
                            else
                            {

                                UserInfoDao dao = new UserInfoDao();

                                WXUserInfo wXUserInfo = new WXUserInfo();
                                wXUserInfo.openId = sessionKeyObj.openid;
                                wXUserInfo.unionId = sessionKeyObj.unionid;
                                wXUserInfo = dao.CreateOrUpdateUnionID(wXUserInfo, qAppId);


                                if (wXUserInfo != null)
                                {
                                    //成功
                                    result.code = 200;
                                    result.message = "success";
                                    respo.openid = sessionKeyObj.openid;
                                    respo.unionid = sessionKeyObj.unionid;
                                    respo.sessionkey = sessionKeyObj.session_key;
                                    result.detail = respo;
                                }
                                else
                                {
                                    //成功
                                    result.code = 401;
                                    result.message = "write to database failed";
                                }

                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.message = ex.Message;
                result.detail = respo;
                return result;
            }

            return result;
        }
        // POST api/<controller>
        [WeixinOnlyFilter]
        public Message<WXUserInfo> Post([FromBody] WXUserInfoReq wxuser)
        {
            Message<WXUserInfo> result = new Message<WXUserInfo>();
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
                else
                {
                    result.code = 400;
                    result.message = "empty wx app id";
                    result.detail = null;
                    return result;
                }
            }

            string wxspSecret = Common.GetAppSettingByKey(qAppId);

            //判断app有效
            if (wxspSecret == null || wxspSecret.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid wx appid";
                result.detail = null;
                return result;
            }

            if (wxuser.iv == null || wxuser.iv.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid iv";
                result.detail = null;
                return result;
            }
            if (wxuser.iv.Length < 20 || wxuser.iv.Length > 40)
            {
                result.code = 400;
                result.message = "invalid iv";
                result.detail = null;
                return result;
            }
            if (wxuser.ed == null || wxuser.ed.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid encryptedData";
                result.detail = null;
                return result;
            }
            if (wxuser.ed.Length < 100 || wxuser.ed.Length > 2000)
            {
                result.code = 400;
                result.message = "invalid encryptedData";
                result.detail = null;
                return result;
            }
            //登录凭证不能为空
            if (wxuser.sk == null || wxuser.sk.IsEmpty())
            {
                result.code = 400;
                result.message = "invalid session_key";
                result.detail = null;
                return result;
            }
            if (wxuser.sk.Length < 20 || wxuser.sk.Length > 40)
            {
                result.code = 400;
                result.message = "invalid session_key";
                result.detail = null;
                return result;
            }

            //open id
            if (wxuser.oid == null || wxuser.oid.IsEmpty())
            {
            }
            else
            {
                if (wxuser.oid.Length < 20 || wxuser.oid.Length > 50)
                {
                    result.code = 400;
                    result.message = "invalid openid";
                    result.detail = null;
                    return result;
                }
            }

            string userInfoDes = KaiPanLaCommon.Security.WXAESDecrypt(wxuser.iv, wxuser.sk, wxuser.ed);
            if (userInfoDes == null)
            {
                result.code = 400;
                result.message = "decrypt user info error, check input param";
                result.detail = null;
                return result;
            }
            WXUserInfo wXUserInfo = JsonConvert.DeserializeObject<WXUserInfo>(userInfoDes);

            if (wXUserInfo == null)
            {
                result.code = 400;
                result.message = "decrypt user info error, invalid user info";
                result.detail = null;
                return result;
            }

            if (String.IsNullOrEmpty(wXUserInfo.openId))
            {
                if (wxuser.oid == null || wxuser.oid.IsEmpty())
                {
                    result.code = 400;
                    result.message = "empty openid in post body";
                    result.detail = null;
                    return result;
                }
                else
                {
                    wXUserInfo.openId = wxuser.oid;
                }
            }

            UserInfoDao dao = new UserInfoDao();
            WXUserInfo dao_res = dao.CreateOrUpdateDetail(wXUserInfo, qAppId);
            if (dao_res != null)
            {
                result.code = 200;
                result.message = "success";
                result.detail = dao_res;
            }
            else
            {
                result.code = 401;
                result.message = "write userinfo to database failure";
                result.detail = wXUserInfo;
            }


            return result;
        }
    }
}
