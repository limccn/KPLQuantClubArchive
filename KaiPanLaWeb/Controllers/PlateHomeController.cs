using System.Web.Mvc;

namespace KaiPanLaWeb.Controllers
{
    public class PlateHomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "板块数据";
            ViewBag.Version = "ver0.1 build 20221122";

            return View();
        }
        public ActionResult PlateStockPage()
        {
            ViewBag.Title = "板块个股数据";
            ViewBag.Version = "ver0.1 build 20221122";

            return View();
        }

        public ActionResult PlateSignalPage()
        {
            ViewBag.Title = "板块实时数据";
            ViewBag.Version = "ver0.1 build 20221122";

            return View();
        }

        public ActionResult PlateSignalDetailPage()
        {
            ViewBag.Title = "板块详细数据";
            ViewBag.Version = "ver0.1 build 20221122";

            return View();
        }
    }
}
