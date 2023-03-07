using KaiPanLaCommon;
using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class PlateStockController : ApiController
    {

        public Logger logger = Logger._;

        // GET api/<controller>
        public IEnumerable<PlateStock> Get(string plateid)
        {
            return this.Get(plateid, 20);
        }

        // GET api/<controller>/ttl
        public IEnumerable<PlateStock> Get(string plateid, Int32 count)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(plateid, count, qDate);

        }

        public IEnumerable<PlateStock> Get(string plateid, Int32 count, Int32 date)
        {

            Int32 qTime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            return this.Get(plateid, count, date, qTime);
        }

        public IEnumerable<PlateStock> Get(string plateid, Int32 count, Int32 date, Int32 time)
        {

            Int32 qCount = count;
            if (count <= 0 || count > 50)
            {
                qCount = 50;
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);

            int qTime = Common.ComputeTime(time);

            IEnumerable<PlateStock> list = this.GetWithParam(plateid, qDate, qTime, qCount);
            if (list == null)
            {
                return new List<PlateStock>();
            }
            return list;
        }

        private IEnumerable<PlateStock> GetWithParam(string plateid, DateTime date, int time, Int32 count)
        {
            PlateStockDao dao = new PlateStockDao();
            List<PlateStock> stocks = dao.QueryPlateStocks(plateid, date, time, count);

            return stocks;
        }

    }
}