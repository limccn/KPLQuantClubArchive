using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class SimulateTradeController : ApiController
    {

        public IEnumerable<SimulateTradeDetail> Get()
        {
            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(qDate, 20, "A_JQ", "C_LMLH888", "ONGO", "1000");
        }

        public IEnumerable<SimulateTradeDetail> Get(Int32 count)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(qDate, count, "A_JQ", "C_LMLH888", "ONGO", "1000");

        }

        public IEnumerable<SimulateTradeDetail> Get(Int32 count, Int32 date)
        {
            return this.Get(date, count, "A_JQ", "C_LMLH888", "ONGO", "1000");
        }

        public IEnumerable<SimulateTradeDetail> Get(string appid, string channelid, string type)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(qDate, 20, appid, channelid, type, "1000");

        }

        public IEnumerable<SimulateTradeDetail> Get(string appid, string channelid, string type, string tcode)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(qDate, 20, appid, channelid, type, tcode);

        }

        public IEnumerable<SimulateTradeDetail> Get(Int32 count, string appid, string channelid, string type, string tcode)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(qDate, count, appid, channelid, type, tcode);

        }

        public IEnumerable<SimulateTradeDetail> Get(Int32 date, Int32 count, string appid, string channelid, string type, string tcode)
        {

            if (count <= 0 || count > 50)
            {
                count = 20;
            }

            if (String.IsNullOrEmpty(appid))
            {
                return new List<SimulateTradeDetail>();
            }

            if (appid.Length > 20 || !appid.StartsWith("A"))
            {
                return new List<SimulateTradeDetail>();
            }

            if (String.IsNullOrEmpty(channelid))
            {
                return new List<SimulateTradeDetail>();
            }

            if (channelid.Length > 20 || !channelid.StartsWith("C"))
            {
                return new List<SimulateTradeDetail>();
            }

            if (String.IsNullOrEmpty(type))
            {
                return new List<SimulateTradeDetail>();
            }

            if (!(type.Equals("BACKTEST") || type.Equals("SIMULATE") || type.Equals("ONGO") || type.Equals("RUNTEST")))
            {
                return new List<SimulateTradeDetail>();
            }

            string trade_type;

            if (tcode.Equals("1000"))
            {
                trade_type = "BUY";
            }
            else if (tcode.Equals("3000"))
            {
                trade_type = "SELL";
            }
            else
            {
                return new List<SimulateTradeDetail>();
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);

            IEnumerable<SimulateTradeDetail> list = this.GetWithParam(appid, channelid, type, qDate, count, trade_type);
            if (list == null)
            {
                return new List<SimulateTradeDetail>();
            }
            return list;

        }

        private IEnumerable<SimulateTradeDetail> GetWithParam(string appid, string channelid, string type, DateTime date, int count, string trade_type)
        {
            SimulateTradeDao dao = new SimulateTradeDao();
            List<SimulateTradeDetail> simus = dao.QuerySimulateTrade(appid, channelid, type, date, count, trade_type);

            return simus;
        }


        public Message<SimulateTrade> Post([FromBody] SimulateTradeReq simulate, string appid, string channelid, string type)
        {
            Message<SimulateTrade> result = new Message<SimulateTrade>();

            if (String.IsNullOrEmpty(appid))
            {
                result.code = 400;
                result.message = "invalid appid";
                result.detail = null;
                return result;
            }

            if (appid.Length > 20 || !appid.StartsWith("A"))
            {
                result.code = 400;
                result.message = "invalid appid";
                result.detail = null;
                return result;
            }

            if (String.IsNullOrEmpty(channelid))
            {
                result.code = 400;
                result.message = "invalid channelid";
                result.detail = null;
                return result;
            }

            if (channelid.Length > 20 || !channelid.StartsWith("C"))
            {
                result.code = 400;
                result.message = "invalid channelid";
                result.detail = null;
                return result;
            }

            if (String.IsNullOrEmpty(type))
            {
                result.code = 400;
                result.message = "invalid type";
                result.detail = null;
                return result;
            }

            if (!(type.Equals("BACKTEST") || type.Equals("SIMULATE") || type.Equals("ONGO") || type.Equals("RUNTEST")))
            {
                result.code = 400;
                result.message = "invalid type";
                result.detail = null;
                return result;
            }

            if (type.Equals("TEST"))
            {
                result.code = 200;
                result.message = "success";
                result.detail = null;
                return result;
            }


            if (String.IsNullOrEmpty(simulate.DATE))
            {
                result.code = 400;
                result.message = "invalid date";
                result.detail = null;
                return result;
            }

            if (simulate.DATE.Length != 8)
            {
                result.code = 400;
                result.message = "invalid date";
                result.detail = null;
                return result;
            }

            if (String.IsNullOrEmpty(simulate.TIME))
            {
                result.code = 400;
                result.message = "invalid time";
                result.detail = null;
                return result;
            }

            if (simulate.TIME.Length != 6)
            {
                result.code = 400;
                result.message = "invalid date";
                result.detail = null;
                return result;
            }

            if (String.IsNullOrEmpty(simulate.CODE))
            {
                result.code = 400;
                result.message = "invalid code";
                result.detail = null;
                return result;
            }

            if (simulate.CODE.Length != 6)
            {
                result.code = 400;
                result.message = "invalid code";
                result.detail = null;
                return result;
            }

            if (simulate.NONCE <= 0)
            {
                result.code = 400;
                result.message = "invalid nonce";
                result.detail = null;
                return result;
            }
            if (String.IsNullOrEmpty(simulate.CODE))
            {
                result.code = 400;
                result.message = "invalid Code";
                result.detail = null;
                return result;
            }

            SimulateTradeDao dao = new SimulateTradeDao();
            int res = dao.Create(simulate, appid, channelid, type);
            if (res > 0)
            {
                result.code = 200;
                result.message = "success";
                result.detail = null;
            }
            else
            {
                result.code = 401;
                result.message = "write trade to database failure";
                result.detail = null;
            }



            return result;
        }


    }
}
