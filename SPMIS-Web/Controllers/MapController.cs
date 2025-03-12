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
            var map = _context.StrategyMaps.FirstOrDefault(m => m.MapId == id);
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
            if (ModelState.IsValid)
            {
                model.MapId = Guid.NewGuid();
                _context.StrategyMaps.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("StrategicMap"); // Redirects to the view that lists maps
            }
            return View(model);
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
    }
}
