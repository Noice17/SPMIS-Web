using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMIS_Web.Data.DataAccessLayer;
using SPMIS_Web.Models.Entities;
using System.Data;

namespace SPMIS_Web.Controllers
{
    public class ObjectiveController : Controller
    {
        private readonly ObjectiveService _objectiveService;
        public ObjectiveController(ObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }
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


        //Create
        [HttpGet]
        public IActionResult AddObjectiveType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddObjectiveType(ObjectiveType objectiveType)
        {

            if (!ModelState.IsValid)
            {
                return View(objectiveType);
            }

            await _objectiveService.AddObjectiveType(objectiveType);
            return RedirectToAction("AddObjectiveType", "Objective");
        }

        //Read/Retrieve (Get data from Database)
        [HttpGet]
        public async Task<IActionResult> ObjectiveTypeList()
        {
            var objectiveTypes = await _objectiveService.GetObjectiveTypes();
            return View(objectiveTypes);
        }

        //Edit
        [HttpPost]
        public async Task<IActionResult> EditObjectiveType([FromBody] ObjectiveType model)
        {
            if (model == null || model.ObjectiveTypeId == Guid.Empty || string.IsNullOrWhiteSpace(model.ObjectiveTypeName))
            {
                return BadRequest(new { message = "Invalid data." });
            }

            var success = await _objectiveService.UpdateObjectiveType(model.ObjectiveTypeId, model.ObjectiveTypeName);
            if (!success)
            {
                return StatusCode(500, new { message = "Error updating Objective Type." });
            }

            return Json(new { message = "Objective Type updated successfully!" });
        }



    }
}
