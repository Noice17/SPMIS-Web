using Microsoft.AspNetCore.Mvc;
using SPMIS_Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using SPMIS_Web.Data;

namespace SPMIS_Web.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StrategicMap()
        {
            ViewData["ActivePage"] = "StrategyMap"; // Highlight Strategy Map

            var maps = _context.StrategyMaps.OrderByDescending(m => m.MapStart).ToList();
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
                return NotFound(); // Handle case when map is not found
            }

            return View(map);
        }

        [HttpGet]
        public IActionResult CreateMap()
        {
            return PartialView("CreateMap"); // Use a Partial View
        }

        [HttpPost]
        public async Task<IActionResult> CreateMap(StrategyMap model)
        {
            // Check if this would be an active map
            bool wouldBeActive = model.MapStart <= DateTime.Now && model.MapEnd > DateTime.Now;

            if (wouldBeActive)
            {
                // Check if there's already an active map
                bool existingActiveMap = await _context.StrategyMaps.AnyAsync(m =>
                    m.MapStart <= DateTime.Now &&
                    m.MapEnd > DateTime.Now);

                if (existingActiveMap)
                {
                    ModelState.AddModelError("", "Only one active strategy map is allowed. Please adjust the dates.");
                    return PartialView("CreateMap", model);
                }
            }

            if (ModelState.IsValid)
            {
                model.MapId = Guid.NewGuid();
                _context.StrategyMaps.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("StrategicMap");
            }
            return PartialView("CreateMap", model);
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

        [HttpGet]
        public async Task<IActionResult> CheckActiveMapExists(Guid? excludeId = null)
        {
            IQueryable<StrategyMap> query = _context.StrategyMaps.Where(m =>
                m.MapStart <= DateTime.Now &&
                m.MapEnd > DateTime.Now);

            // If we're editing an existing map, exclude it from the check
            if (excludeId.HasValue)
            {
                query = query.Where(m => m.MapId != excludeId.Value);
            }

            bool existingActiveMap = await query.AnyAsync();

            return Json(new { exists = existingActiveMap });
        }
    }


}
