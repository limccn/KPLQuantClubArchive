using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeixinMiniUser.Models
{
    public class Message<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T detail { get; set; }
    }
}