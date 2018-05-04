using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;
using WebInterface.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WebInterface.Controllers
{
    public class DishTypesController : Controller
    {
        private readonly MenuCardDBContext _context;

        public DishTypesController(MenuCardDBContext context)
        {
            _context = context;
        }

        // GET: DishTypes
        public async Task<IActionResult> Index()
        {
            //DishType dishType = new DishType
            //{
            //    dishType.SubType = "Test";
            //}



            var model = await _context.DishTypes.ToListAsync();
            await _context.SubDishTypes.ToListAsync();

            return View(model);
        }

        // GET: DishTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishType = await _context.DishTypes
                .SingleOrDefaultAsync(m => m.DishTypeID == id);
            if (dishType == null)
            {
                return NotFound();
            }
            await _context.SubDishTypes.ToListAsync();

            return View(dishType);
        }

        // GET: DishTypes/Create
        public IActionResult Create()
        {
            _context.Database.EnsureCreated();

            var ListOfSubDishTypes = _context.SubDishTypes;

            DishTypesViewModel model = new DishTypesViewModel
            {
                SubTypeList = ListOfSubDishTypes.ToList()
            };

            return View(model);
        }

        // POST: DishTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind("Dish, Dish.Name, Dish.Course, Ingredients, Dish.DishTypeID, SubTypeID, Dish.Recipe, Dish.Price")]DishTypesViewModel dishTypeViewModel)
        {
            //HEAD
            if (ModelState.IsValid)
            {
                //
                DishType model = new DishType
                {
                    Name = dishTypeViewModel.Dish.Name,
                    Course = dishTypeViewModel.Dish.Course,
                    Recipe = dishTypeViewModel.Dish.Recipe,
                    Price = dishTypeViewModel.Dish.Price
                };

                DbSet<SubDishType> subDishTypes = _context.SubDishTypes;

                var query = subDishTypes.Where(s => dishTypeViewModel.SubTypeID == s.SubDishTypeID);
                SubDishType sdt = query.FirstOrDefault();

                if (sdt == null)
                {
                    return NotFound();
                }


                model.SubType = sdt;

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }            

            return View(dishTypeViewModel);
        }

        // GET: DishTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var dishTypesViewModel = new DishTypesViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var dishType = await _context.DishTypes.SingleOrDefaultAsync(m => m.DishTypeID == id);
            if (dishType == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes.ToListAsync();

            var subCourseType = await _context.SubDishTypes.ToListAsync();

            dishTypesViewModel.Dish = dishType;
            dishTypesViewModel.Ingredients = ingredientType;
            dishTypesViewModel.SubTypeList = subCourseType;

            return View(dishTypesViewModel);
        }

        // POST: DishTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DishTypesViewModel dishType)
        {
            if (ModelState.IsValid)
            {
                if (dishType.IngredientToAdd != null)
                {
                    //Ingredient ingredient;
                    //ingredient = new Ingredient();
                    Ingredient ingredient = new Ingredient
                    {
                        Type = dishType.IngredientToAdd,
                        Quantity = 0
                    };

                    _context.Ingredients.Add(ingredient);
                    await _context.SaveChangesAsync();

                    if (dishType.Dish.DefaultIngredients ==null)
                    {
                        dishType.Dish.DefaultIngredients = new List<Ingredient>();
                    }
                    dishType.Dish.DefaultIngredients.Add(ingredient);
                }

                try
                {
                    _context.Update(dishType.Dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishTypeExists(dishType.Dish.DishTypeID))
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
            return View(dishType);
        }

        // GET: DishTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishType = await _context.DishTypes
                .SingleOrDefaultAsync(m => m.DishTypeID == id);
            if (dishType == null)
            {
                return NotFound();
            }

            return View(dishType);
        }

        // POST: DishTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishType = await _context.DishTypes.SingleOrDefaultAsync(m => m.DishTypeID == id);
            _context.DishTypes.Remove(dishType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishTypeExists(int id)
        {
            return _context.DishTypes.Any(e => e.DishTypeID == id);
        }
    }
}
