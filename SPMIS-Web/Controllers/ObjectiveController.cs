using Microsoft.AspNetCore.Mvc;

namespace SPMIS_Web.Controllers
{
    public class ObjectiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddObjective()
        {
            return View();
        }
        //[HttpPost]
        [HttpGet]
        public IActionResult AddObjectiveType()
        {
            return View();
        }
        //[HttpPost]
    }
}
