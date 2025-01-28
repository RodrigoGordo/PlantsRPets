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
    public class SustainabilityTipsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public SustainabilityTipsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: SustainabilityTips
        public async Task<IActionResult> Index()
        {
            return View(await _context.SustainabilityTip.ToListAsync());
        }

        // GET: SustainabilityTips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sustainabilityTip = await _context.SustainabilityTip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sustainabilityTip == null)
            {
                return NotFound();
            }

            return View(sustainabilityTip);
        }

        // GET: SustainabilityTips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SustainabilityTips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Category,AuthorId")] SustainabilityTip sustainabilityTip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sustainabilityTip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sustainabilityTip);
        }

        // GET: SustainabilityTips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sustainabilityTip = await _context.SustainabilityTip.FindAsync(id);
            if (sustainabilityTip == null)
            {
                return NotFound();
            }
            return View(sustainabilityTip);
        }

        // POST: SustainabilityTips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Category,AuthorId")] SustainabilityTip sustainabilityTip)
        {
            if (id != sustainabilityTip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sustainabilityTip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SustainabilityTipExists(sustainabilityTip.Id))
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
            return View(sustainabilityTip);
        }

        // GET: SustainabilityTips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sustainabilityTip = await _context.SustainabilityTip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sustainabilityTip == null)
            {
                return NotFound();
            }

            return View(sustainabilityTip);
        }

        // POST: SustainabilityTips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sustainabilityTip = await _context.SustainabilityTip.FindAsync(id);
            if (sustainabilityTip != null)
            {
                _context.SustainabilityTip.Remove(sustainabilityTip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SustainabilityTipExists(int id)
        {
            return _context.SustainabilityTip.Any(e => e.Id == id);
        }
    }
}
