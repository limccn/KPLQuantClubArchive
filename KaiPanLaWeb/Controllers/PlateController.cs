using KaiPanLaCommon;
using KaiPanLaWeb.Daos;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KaiPanLaWeb.Controllers
{
    public class PlateController : ApiController
    {

        public Logger logger = Logger._;


        // GET api/<controller>
        public IEnumerable<Plate> Get()
        {
            return this.Get(20);
        }

        // GET api/<controller>/
        public IEnumerable<Plate> Get(Int32 count)
        {
            Int32 qDate = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return this.Get(count, qDate);
        }


        public IEnumerable<Plate> Get(Int32 count, Int32 date)
        {
            Int32 qTime = Int32.Parse(DateTime.Now.ToString("HHmm"));
            return this.Get(count, date, qTime);
        }

        public IEnumerable<Plate> Get(Int32 count, Int32 date, Int32 time)
        {

            Int32 qCount = count;
            if (count <= 0 || count > 20)
            {
                qCount = 20;
            }

            DateTime qDate = DateTime.Now;
            qDate = Common.ComputeDate(date);

            int qTime = Common.ComputeTime(time);


            IEnumerable<Plate> list = this.GetWithParam(qDate, qTime, qCount);
            if (list == null)
            {
                return new List<Plate>();
            }
            return list;
        }

        private IEnumerable<Plate> GetWithParam(DateTime date, int time, Int32 count)
        {
            PlateDao dao = new PlateDao();
            List<Plate> plates = dao.QueryPlates(date, time, count);

            return plates;
        }

    }
}