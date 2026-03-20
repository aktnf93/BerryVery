using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route
{
    [Route("[Controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            base.ViewData["ControllerIndex"] = "안녕하세요!";

            return base.View(); // Project/Views/Home/Index.cshtml
        }
    }
}


