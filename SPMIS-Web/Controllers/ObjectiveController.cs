using Microsoft.AspNetCore.Mvc;
using SPMIS_Web.Models.Entities;
using SPMIS_Web.Models.ViewModels;
using SPMIS_Web.Data.DataAccessLayer;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        // GET: Load Add Objective form with dropdown values
        [HttpGet]
        public async Task<IActionResult> AddObjective(Guid mapId)
        {
            var objectiveTypes = await _objectiveService.GetObjectiveTypes();

            var model = new AddObjectiveViewModel
            {
                MapId = mapId,
                ObjectiveType = objectiveTypes.Select(o => new ObjectiveType
                {
                    ObjectiveTypeId = o.ObjectiveTypeId,
                    ObjectiveTypeName = o.ObjectiveTypeName
                }).ToList()
            };

            return PartialView("AddObjective", model); //Load as partial view
        }

        // POST: Add Objective
        [HttpPost]
        public async Task<IActionResult> AddObjective([FromBody] AddObjectiveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            var newObjective = new Objective
            {
                ObjectiveDescription = model.ObjectiveDescription,
                ObjectiveTypeId = model.ObjectTypeId,
                MapId = model.MapId
            };

            await _objectiveService.AddObjective(newObjective);

            return Json(new { success = true, message = "Objective added successfully!" });
            //return RedirectToAction("_AddObjective", "Map");
        }

        // GET: Update Objective
        [HttpGet]
        public async Task<IActionResult> UpdateObjective(Guid objectiveId)
        {
            var objectiveTypes = await _objectiveService.GetObjectiveTypes();

            if (objectiveId == Guid.Empty)
            {
                return BadRequest("Invalid Objective ID");
            }

            var objective = await _objectiveService.GetObjectiveByIdAsync(objectiveId);

            if (objective == null)
            {
                return NotFound("Objective not found");
            }

            var viewModel = new AddObjectiveTypeViewModel
            {
                ObjectiveDescription = objective.ObjectiveDescription,
                ObjectiveType = objectiveTypes.Select(o => new ObjectiveType
                {
                    ObjectiveTypeId = o.ObjectiveTypeId,
                    ObjectiveTypeName = o.ObjectiveTypeName
                }).ToList(),
                MapId = objective.MapId
            };

            return PartialView("UpdateObjective", viewModel);
        }








        // GET: Load Add Objective Type form
        [HttpGet]
        public IActionResult AddObjectiveType()
        {
            return View();
        }

        // POST: Add Objective Type
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

        // GET: Retrieve list of Objective Type
        [HttpGet]
        public async Task<IActionResult> ObjectiveTypeList()
        {
            var objectiveType = await _objectiveService.GetObjectiveTypes();
            return View(objectiveType);
        }

        // POST: Edit Objective Type
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
