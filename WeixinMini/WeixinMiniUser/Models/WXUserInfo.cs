using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeixinMiniUser.Models
{
    public class WXUserInfoReq
    {
        public string ed { get; set; }
        public string iv { get; set; }
        public string sk { get; set; }

    }

    public class UserInfo
    {
        public string openid { get; set; }
        public string unionid { get; set; }
        public string sessionkey { get; set; }

    }

    public class UserAccessToken
    {
        public string open_id { get; set; }
        public string union_id { get; set; }
        public string access_token { get; set; }
        public Int64 expire { get; set; }
    }

    public class UserSubscribe
    {
        public string open_id { get; set; }
        public string union_id { get; set; }
        public string sub_type { get; set; }
        public Int64 sub_expire { get; set; }
    }

    public class WXSessionKey
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }

    }

    public class WXUserInfo
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public string unionId { get; set; }

    }
}