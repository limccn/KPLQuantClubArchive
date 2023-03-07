using KaiPanLaCommon;
using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class PlateSignalDetailController : ApiController
    {

        public Logger logger = Logger._;

        // GET api/<controller>
        public IEnumerable<PlateSignalDetail> Get()
        {
            return this.Get(50);
        }

        // GET api/<controller>/ttl
        public IEnumerable<PlateSignalDetail> Get(Int32 count)
        {
            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(count, qDate);
        }

        public IEnumerable<PlateSignalDetail> Get(Int32 count, Int32 date)
        {
            Int32 qTime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            return this.Get(count, date, qTime);
        }
        public IEnumerable<PlateSignalDetail> Get(Int32 count, Int32 date, int time)
        {
            Int32 qCount = count;
            if (count <= 0 || count > 50)
            {
                qCount = 50;
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);

            int qTime = Common.ComputeTime(time);

            IEnumerable<PlateSignalDetail> list = this.GetWithParam(qDate, qTime, qCount);
            if (list == null)
            {
                return new List<PlateSignalDetail>();
            }
            return list;
        }

        private IEnumerable<PlateSignalDetail> GetWithParam(DateTime date, Int32 time, Int32 count)
        {
            PlateSignalDao dao = new PlateSignalDao();

            List<PlateSignalDetail> signals = dao.QueryPlateSignalDetails(date, time, count);

            return signals;
        }
    }
}