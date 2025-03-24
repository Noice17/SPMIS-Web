using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPMIS_Web.Data;
using SPMIS_Web.Models.Entities;
using SPMIS_Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace SPMIS_Web.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    ViewData["ActivePage"] = "Index"; // Highlight Strategy Map

        //    var maps = _context.StrategyMaps
        //        .OrderByDescending(m => m.MapStart)
        //        .ToList();

        //    return View(maps);
        //}

        public ActionResult Index()
        {
            var activeMap = _context.StrategyMaps.FirstOrDefault(m => m.IsActive); // Find active map

            if (activeMap != null)
            {
                return RedirectToAction("ViewMap", "Map", new { id = activeMap.MapId });
            }

            return RedirectToAction("Index", "Home"); // Redirect to Home if no active map exists
        }





        [HttpGet]
        public IActionResult StrategicMap()
        {
            ViewData["ActivePage"] = "StrategyMap"; // Highlight Strategy Map

            var maps = _context.StrategyMaps
                .OrderByDescending(m => m.MapStart)
                .ToList();

            return View(maps);
        }

        [HttpGet]
        public IActionResult ViewMap(Guid id)
        {
            var map = _context.StrategyMaps
                .Include(m => m.Objective) // Include the related Objective data
                .ThenInclude(o => o.Type)
                .FirstOrDefault(m => m.MapId == id);

            if (map == null)
            {
                return NotFound();
            }

            // Ensure we pass the correct ViewModel
            var viewModel = new AddObjectiveTypeViewModel
            {
                MapId = map.MapId,
                MapTitle = map.MapTitle,
                MapDescription = map.MapDescription,
                MapStart = map.MapStart,
                MapEnd = map.MapEnd,
                Objective = map.Objective?.ToList() ?? new List<Objective>()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateMap()
        {
            return PartialView("CreateMap"); // Load as Partial View
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateMap(StrategyMap model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    model.MapId = Guid.NewGuid();
        //    _context.StrategyMaps.Add(model);
        //    await _context.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Strategy Map created successfully!";
        //    return RedirectToAction("StrategicMap");
        //}
        [HttpPost]
        public async Task<IActionResult> CreateMap(StrategyMap model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get the current date
            DateTime currentDate = DateTime.Today;

            // Ensure start and end dates are used correctly
            DateTime mapStartDate = model.MapStart;
            DateTime mapEndDate = model.MapEnd;

            // Check if there is an active map that overlaps with the selected date range
            bool hasActiveMapInRange = await _context.StrategyMaps
                .AnyAsync(m => m.IsActive == true &&
                               ((m.MapStart <= mapEndDate && m.MapEnd >= mapStartDate)));

            // Determine IsActive status:
            // - If the new map's date range has already ended, it must be inactive.
            // - Otherwise, it's active only if no other active maps exist in the range.
            model.IsActive = (currentDate <= mapEndDate) && !hasActiveMapInRange;

            // Assign a new ID
            model.MapId = Guid.NewGuid();

            // Save to database
            _context.StrategyMaps.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Strategy Map created successfully!";
            return RedirectToAction("StrategicMap");
        }





        [HttpGet]
        public IActionResult MapList(int page = 1)
        {
            int pageSize = 10;
            var query = _context.StrategyMaps.OrderByDescending(m => m.MapStart);
            var count = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (count + pageSize - 1) / pageSize;

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMap(Guid mapId)
        {
            var map = await _context.StrategyMaps
                .Where(m => m.MapId == mapId)
                .Select(m => new StrategyMap
                {
                    MapId = m.MapId,
                    MapTitle = m.MapTitle,
                    MapDescription = m.MapDescription,
                    MapStart = m.MapStart,
                    MapEnd = m.MapEnd,
                    IsActive = m.IsActive
                }).FirstOrDefaultAsync();

            if (map == null)
            {
                return NotFound();
            }

            return PartialView("UpdateMap", map);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMap(StrategyMap model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var map = await _context.StrategyMaps.FindAsync(model.MapId);
            if (map == null)
            {
                return NotFound();
            }

            // Check if the user is trying to activate this map
            if (model.IsActive)
            {
                bool isAnotherMapActive = await _context.StrategyMaps.AnyAsync(m => m.IsActive && m.MapId != model.MapId);

                if (isAnotherMapActive)
                {
                    return BadRequest(new { error = "Another active map already exists. Please deactivate it first." });
                }
            }

            map.MapTitle = model.MapTitle;
            map.MapDescription = model.MapDescription;
            map.MapStart = model.MapStart;
            map.MapEnd = model.MapEnd;
            map.IsActive = model.IsActive;

            _context.StrategyMaps.Update(map);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMap(StrategyMap strategyMap)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMap = await _context.StrategyMaps.FindAsync(strategyMap.MapId);

                    if (existingMap == null)
                    {
                        return NotFound();
                    }

                    // Check if this would be an active map after editing
                    bool wouldBeActive = strategyMap.MapStart <= DateTime.Now && strategyMap.MapEnd > DateTime.Now;

                    if (wouldBeActive)
                    {
                        // Check if there's already a different active map
                        bool existingActiveMap = await _context.StrategyMaps.AnyAsync(m =>
                            m.MapId != strategyMap.MapId &&
                            m.MapStart <= DateTime.Now &&
                            m.MapEnd > DateTime.Now);

                        if (existingActiveMap)
                        {
                            ModelState.AddModelError("", "Only one active strategy map is allowed. Please adjust the dates.");
                            return View("ViewMap", existingMap);
                        }
                    }

                    // Update only the properties that can be edited
                    existingMap.MapTitle = strategyMap.MapTitle;
                    existingMap.MapDescription = strategyMap.MapDescription;
                    existingMap.MapStart = strategyMap.MapStart;
                    existingMap.MapEnd = strategyMap.MapEnd;

                    _context.Update(existingMap);
                    await _context.SaveChangesAsync();

                    // Redirect to the strategy map view
                    return RedirectToAction("ViewMap", "Map", new { id = strategyMap.MapId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.StrategyMaps.Any(e => e.MapId == strategyMap.MapId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If we got this far, something failed, redisplay the strategy map
            return RedirectToAction("ViewMap", new { id = strategyMap.MapId });
        }



        // ---- DO NOT USE: Function is moved to AddObjective in the Objective Controller  ---
        //[HttpGet]
        //public IActionResult AddObjectivePartial(Guid MapId)
        //{
        //    var viewModel = new AddObjectiveTypeViewModel
        //    {
        //        MapId = MapId,
        //        ObjectiveType = _context.ObjectiveTypes
        //            .Select(o => new ObjectiveType
        //            {
        //                ObjectiveTypeId = o.ObjectiveTypeId,
        //                ObjectiveTypeName = o.ObjectiveTypeName
        //            }).ToList()
        //    };

        //    return PartialView("_AddObjectivePartial", viewModel);
        //}

    }
}