using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class IngredientTypesController : Controller
    {
        private readonly MenuCardDBContext _context;

        public IngredientTypesController(MenuCardDBContext context)
        {
            _context = context;

			_context.Database.EnsureCreated();
        }

        // GET: IngredientTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.IngredientTypes.ToListAsync());
        }

        // GET: IngredientTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes
                .SingleOrDefaultAsync(m => m.IngredientTypeID == id);
            if (ingredientType == null)
            {
                return NotFound();
            }

            return View(ingredientType);
        }

        // GET: IngredientTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IngredientTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientTypeID,Name,UnitSingular,UnitPlural")] IngredientType ingredientType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredientType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientType);
        }

        // GET: IngredientTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes.SingleOrDefaultAsync(m => m.IngredientTypeID == id);
            if (ingredientType == null)
            {
                return NotFound();
            }
            return View(ingredientType);
        }

        // POST: IngredientTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredientTypeID,Name,UnitSingular,UnitPlural")] IngredientType ingredientType)
        {
            if (id != ingredientType.IngredientTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredientType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientTypeExists(ingredientType.IngredientTypeID))
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
            return View(ingredientType);
        }

        // GET: IngredientTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes
                .SingleOrDefaultAsync(m => m.IngredientTypeID == id);
            if (ingredientType == null)
            {
                return NotFound();
            }

            return View(ingredientType);
        }

        // POST: IngredientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientType = await _context.IngredientTypes.SingleOrDefaultAsync(m => m.IngredientTypeID == id);
            _context.IngredientTypes.Remove(ingredientType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientTypeExists(int id)
        {
            return _context.IngredientTypes.Any(e => e.IngredientTypeID == id);
        }
    }
}
