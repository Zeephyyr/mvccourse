using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ResultMessage = TempData["ResultMessage"];
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}