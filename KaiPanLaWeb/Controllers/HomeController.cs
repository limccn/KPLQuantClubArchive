using System.Web.Mvc;

namespace KaiPanLaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "实时数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }

        public ActionResult DetailPage()
        {
            ViewBag.Title = "详细数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }


        public ActionResult PortfolioPage()
        {
            ViewBag.Title = "自选数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }

        public ActionResult PortfolioSignalPage()
        {
            ViewBag.Title = "自选信号数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }

        public ActionResult ConfirmPage()
        {
            ViewBag.Title = "信号数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }

        public ActionResult SimulatePage()
        {
            ViewBag.Title = "模拟交易数据";
            ViewBag.Version = "ver0.1 build 20211214";

            return View();
        }
    }
}
