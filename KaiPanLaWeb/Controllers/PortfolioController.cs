using KaiPanLaCommon;
using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class PortfolioController : ApiController
    {

        public Logger logger = Logger._;

        // GET api/<controller>
        public IEnumerable<Portfolio> Get()
        {
            IEnumerable<Portfolio> list = this.GetWithParam(0, DateTime.Now, 20);
            if (list == null)
            {
                return new List<Portfolio>();
            }
            return list;
        }

        public IEnumerable<Portfolio> Get(Int32 ttl)
        {
            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(ttl, 20, qDate);
        }

        // GET api/<controller>/ttl
        public IEnumerable<Portfolio> Get(Int32 ttl, Int32 count)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(ttl, count, qDate);

        }

        public IEnumerable<Portfolio> Get(Int32 ttl, Int32 count, Int32 date)
        {
            Int32 qTTL = ttl;
            if (ttl < 0 || ttl > 3600)
            {
                qTTL = 900;
            }

            Int32 qCount = count;
            if (count <= 0 || count > 50)
            {
                qCount = 20;
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);

            IEnumerable<Portfolio> list = this.GetWithParam(qTTL, qDate, qCount);
            if (list == null)
            {
                return new List<Portfolio>();
            }
            return list;
        }

        private IEnumerable<Portfolio> GetWithParam(Int32 ttl, DateTime date, Int32 count)
        {
            PortfolioDao dao = new PortfolioDao();
            List<Portfolio> portfolios = dao.QueryPortfolios(date, ttl, count);

            return portfolios;
        }

    }
}