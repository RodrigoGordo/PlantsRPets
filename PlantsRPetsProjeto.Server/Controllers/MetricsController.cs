using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    public class MetricsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public MetricsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: Metrics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Metric.ToListAsync());
        }

        // GET: Metrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metric = await _context.Metric
                .FirstOrDefaultAsync(m => m.MetricId == id);
            if (metric == null)
            {
                return NotFound();
            }

            return View(metric);
        }

        // GET: Metrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Metrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MetricId,UserId,TotalPlants,WaterSaved,CarbonFootprintReduction")] Metric metric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metric);
        }

        // GET: Metrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metric = await _context.Metric.FindAsync(id);
            if (metric == null)
            {
                return NotFound();
            }
            return View(metric);
        }

        // POST: Metrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MetricId,UserId,TotalPlants,WaterSaved,CarbonFootprintReduction")] Metric metric)
        {
            if (id != metric.MetricId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetricExists(metric.MetricId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(metric);
        }

        // GET: Metrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metric = await _context.Metric
                .FirstOrDefaultAsync(m => m.MetricId == id);
            if (metric == null)
            {
                return NotFound();
            }

            return View(metric);
        }

        // POST: Metrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metric = await _context.Metric.FindAsync(id);
            if (metric != null)
            {
                _context.Metric.Remove(metric);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetricExists(int id)
        {
            return _context.Metric.Any(e => e.MetricId == id);
        }
    }
}
