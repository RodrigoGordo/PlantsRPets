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
    public class PlantationsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantationsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: Plantations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plantation.ToListAsync());
        }

        // GET: Plantations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantation = await _context.Plantation
                .FirstOrDefaultAsync(m => m.PlantationId == id);
            if (plantation == null)
            {
                return NotFound();
            }

            return View(plantation);
        }

        // GET: Plantations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plantations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlantationId,PlantId,OwnerId,PlantingDate,LastWatered,HarvestDate,GrowthStatus,ExperiencePoints,Level")] Plantation plantation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantation);
        }

        // GET: Plantations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
            {
                return NotFound();
            }
            return View(plantation);
        }

        // POST: Plantations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlantationId,PlantId,OwnerId,PlantingDate,LastWatered,HarvestDate,GrowthStatus,ExperiencePoints,Level")] Plantation plantation)
        {
            if (id != plantation.PlantationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantationExists(plantation.PlantationId))
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
            return View(plantation);
        }

        // GET: Plantations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantation = await _context.Plantation
                .FirstOrDefaultAsync(m => m.PlantationId == id);
            if (plantation == null)
            {
                return NotFound();
            }

            return View(plantation);
        }

        // POST: Plantations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation != null)
            {
                _context.Plantation.Remove(plantation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantationExists(int id)
        {
            return _context.Plantation.Any(e => e.PlantationId == id);
        }
    }
}
