using BerryServer.DTO;
using BerryServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            base.ViewData["ControllerIndex"] = "안녕하세요!";

            base.ViewBag.Status = "작업 완료";

            var user = new User { Name = "Gemini", Age = 20 };
            return base.View(user);
        }

        public ActionResult Save()
        {
            base.TempData["Alert"] = "성공적으로 저장되었습니다!";

            return base.RedirectToAction(nameof(this.Index));
        }

        public ActionResult Details()
        {
            return base.View();
        }

        public ActionResult Test()
        {
            return base.View();
        }
    }
}


