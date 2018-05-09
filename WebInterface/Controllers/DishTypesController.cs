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
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;

namespace WebInterface.Controllers
{
    public class DishTypesController : Controller
    {
        private readonly MenuCardDBContext _context;

        public DishTypesController(MenuCardDBContext context)
        {
            _context = context;

			_context.Database.EnsureCreated();
        }

        // GET: DishTypes
        public async Task<IActionResult> Index()
        {
            var model = await _context.DishTypes.ToListAsync();
            await _context.SubDishTypes.ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> PageData(IDataTablesRequest request)
        {
            var ingredients = await _context.IngredientTypes.Where(x => x.IngredientTypeID <100).ToListAsync();
            var totalRecords = await _context.IngredientTypes.CountAsync();

            return Json(ingredients);

            //var totalRecordsFiltered = ingredients.Count;

        }

        // Add ingredients to the dish
        [HttpPost]
        public IActionResult AddIngredientToDish(int IngredientTypeID, int DishTypeID)
        {
            Ingredient ingredient = new Ingredient();


            DishType P = _context.DishTypes.Where(x => x.DishTypeID == DishTypeID).FirstOrDefault();

            if (P == null)
            {
                return NotFound();
            }

            IngredientType T = _context.IngredientTypes.Where(x => x.IngredientTypeID == IngredientTypeID).FirstOrDefault();

            if (T == null)
            {
                return NotFound();
            }

            _context.Ingredients.ToList();

            ingredient.Type = T;
            
            if (P.DefaultIngredients == null)
            {
                P.DefaultIngredients = new List<Ingredient>();
            }

            P.DefaultIngredients.Add(ingredient);

            _context.SaveChanges();

            return RedirectToAction("Edit", new { id=DishTypeID });
        }

        // Remove ingredients from the dish
        public IActionResult RemoveIngredient (int DishTypeID, int IngredientID)
        {
            var ingredientID = _context.Ingredients.SingleOrDefault(x => x.IngredientID == IngredientID);
            _context.Ingredients.Remove(ingredientID);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = DishTypeID });
        }

        public IActionResult SaveQuantity (int DishTypeID, int IngredientID, int Quantity)
        {
            
            var ingredientQuery = _context.Ingredients.Where(x => x.IngredientID == IngredientID);

            Ingredient ingredient = ingredientQuery.FirstOrDefault();

            ingredient.Quantity = Quantity;
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = DishTypeID });
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

                model.SubDishType = sdt;

                _context.Add(model);
                await _context.SaveChangesAsync();

                int id = model.DishTypeID;
                return RedirectToAction(nameof(Edit), new { id = id });
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

            _context.Ingredients.ToList();

            if (dishType == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes.ToListAsync();

            var subDishType = await _context.SubDishTypes.ToListAsync();

            dishTypesViewModel.Dish = dishType;
            dishTypesViewModel.Ingredients = ingredientType;
            dishTypesViewModel.SubTypeList = subDishType;

            return View("Edit",dishTypesViewModel);
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

            var dishTypesViewModel = new DishTypesViewModel();

            var model = await _context.DishTypes
                .SingleOrDefaultAsync(m => m.DishTypeID == id);

            var subDishType = await _context.SubDishTypes.ToListAsync();
            dishTypesViewModel.SubTypeList = subDishType;

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
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
