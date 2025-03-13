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

        [HttpPost]
        public async Task<IActionResult> CreateMap(StrategyMap model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.MapId = Guid.NewGuid();
            _context.StrategyMaps.Add(model);
            await _context.SaveChangesAsync();

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
        public IActionResult AddObjectivePartial(Guid MapId)
        {
            var viewModel = new AddObjectiveTypeViewModel
            {
                MapId = MapId,
                ObjectiveType = _context.ObjectiveTypes
                    .Select(o => new ObjectiveType
                    {
                        ObjectiveTypeId = o.ObjectiveTypeId,
                        ObjectiveTypeName = o.ObjectiveTypeName
                    }).ToList()
            };

            return PartialView("_AddObjectivePartial", viewModel);
        }

    }
}