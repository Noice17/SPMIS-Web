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

            var viewModel = new UpdateObjectiveViewModel
            {
                ObjectiveId = objectiveId,
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

        [HttpPost]
        public async Task<IActionResult> UpdateObjective([FromBody] UpdateObjectiveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid input data." });
            }

            var existingObjective = await _objectiveService.GetObjectiveByIdAsync(model.ObjectiveId);
            if (existingObjective == null)
            {
                return NotFound(new { success = false, message = "Objective not found." });
            }

            existingObjective.ObjectiveDescription = model.ObjectiveDescription;
            existingObjective.ObjectiveTypeId = model.ObjectiveTypeId;
            existingObjective.MapId = model.MapId;

            await _objectiveService.UpdateObjectiveAsync(existingObjective);

            return Json(new { success = true, message = "Objective updated successfully!" });
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
