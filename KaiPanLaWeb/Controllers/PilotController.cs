using System.Web.Mvc;

namespace KaiPanLaWeb.Controllers
{
    public class PilotController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "复盘";
            ViewBag.Version = "ver0.1 build 20221214";

            return View();
        }
    }
}
