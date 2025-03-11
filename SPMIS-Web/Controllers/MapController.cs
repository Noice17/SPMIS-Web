using Microsoft.AspNetCore.Mvc;

namespace SPMIS_Web.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StrategicMap()
        {
            return View();
        }
    }
}
