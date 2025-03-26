using Microsoft.AspNetCore.Mvc;
using SPMIS_Web.Data.DataAccessLayer;
using SPMIS_Web.Models.Entities;

namespace SPMIS_Web.Controllers
{
    public class ObjectiveTypeController : Controller
    {
        private readonly ObjectiveService _objectiveService;
        public ObjectiveTypeController(ObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var objectiveType = await _objectiveService.GetObjectiveTypes();
            return View(objectiveType);
        }

        // GET: Load Add Objective Type form
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Add Objective Type
        [HttpPost]
        public async Task<IActionResult> Add(ObjectiveType objectiveType)
        {
            if (!ModelState.IsValid)
            {
                return View(objectiveType);
            }

            await _objectiveService.AddObjectiveType(objectiveType);
            return RedirectToAction("Add", "ObjectiveType");
        }

        // POST: Edit Objective Type
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ObjectiveType model)
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

        // GET: Retrieve Objective Types for JSON response
        [HttpGet]
        public async Task<IActionResult> GetObjectiveTypes()
        {
            var types = await _objectiveService.GetObjectiveTypes();

            var result = types.Select(t => new
            {
                t.ObjectiveTypeId,
                t.ObjectiveTypeName
            }).ToList();

            return Json(result);
        }
    }
}

