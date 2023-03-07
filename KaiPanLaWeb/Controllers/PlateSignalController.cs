using KaiPanLaCommon;
using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class PlateSignalController : ApiController
    {

        public Logger logger = Logger._;

        // GET api/<controller>
        public IEnumerable<PlateSignal> Get()
        {
            return this.Get(50);
        }

        // GET api/<controller>/ttl
        public IEnumerable<PlateSignal> Get(Int32 count)
        {

            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(count, qDate);

        }

        public IEnumerable<PlateSignal> Get(Int32 count, Int32 date)
        {
            Int32 qTime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            return this.Get(count, date, qTime);
        }

        public IEnumerable<PlateSignal> Get(Int32 count, Int32 date, Int32 time)
        {

            Int32 qCount = count;
            if (count <= 0 || count > 50)
            {
                qCount = 20;
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);
            int qTime = Common.ComputeTime(time);

            IEnumerable<PlateSignal> list = this.GetWithParam(qDate, qTime, qCount);
            if (list == null)
            {
                return new List<PlateSignal>();
            }
            return list;
        }

        private IEnumerable<PlateSignal> GetWithParam(DateTime date, int time, Int32 count)
        {
            PlateSignalDao dao = new PlateSignalDao();
            List<PlateSignal> signals = dao.QueryPlateSignals(date, time, count);

            return signals;
        }

    }
}