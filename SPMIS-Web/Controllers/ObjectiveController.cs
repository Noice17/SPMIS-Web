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
