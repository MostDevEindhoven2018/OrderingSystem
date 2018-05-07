using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebInterface.Controllers
{
    [Authorize]
    public class SubDishTypesController : Controller
    {
        private readonly MenuCardDBContext _context;

        public SubDishTypesController(MenuCardDBContext context)
        {
            _context = context;
        }

        // GET: SubDishTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubDishTypes.ToListAsync());
        }

        // GET: SubDishTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subDishType = await _context.SubDishTypes
                .SingleOrDefaultAsync(m => m.SubDishTypeID == id);
            if (subDishType == null)
            {
                return NotFound();
            }

            return View(subDishType);
        }

        // GET: SubDishTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubDishTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubDishTypeID,SubType")] SubDishType subDishType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subDishType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subDishType);
        }

        // GET: SubDishTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subDishType = await _context.SubDishTypes.SingleOrDefaultAsync(m => m.SubDishTypeID == id);
            if (subDishType == null)
            {
                return NotFound();
            }
            return View(subDishType);
        }

        // POST: SubDishTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubDishTypeID,SubType")] SubDishType subDishType)
        {
            if (id != subDishType.SubDishTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subDishType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubDishTypeExists(subDishType.SubDishTypeID))
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
            return View(subDishType);
        }

        // GET: SubDishTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subDishType = await _context.SubDishTypes
                .SingleOrDefaultAsync(m => m.SubDishTypeID == id);
            if (subDishType == null)
            {
                return NotFound();
            }

            return View(subDishType);
        }

        // POST: SubDishTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subDishType = await _context.SubDishTypes.SingleOrDefaultAsync(m => m.SubDishTypeID == id);
            _context.SubDishTypes.Remove(subDishType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubDishTypeExists(int id)
        {
            return _context.SubDishTypes.Any(e => e.SubDishTypeID == id);
        }
    }
}
