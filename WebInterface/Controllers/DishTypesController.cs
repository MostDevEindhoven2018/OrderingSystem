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
using WebInterface.Repositories;

namespace WebInterface.Controllers
{
    public class DishTypesController : Controller
    {
        public DishTypesController(MenuCardDBContext context)
        {
            repo = new DishTypeRepository(context);
        }

        private DishTypeRepository repo = null;

        // GET: DishTypes
        public async Task<IActionResult> Index()
        {
            var model = await repo.GetDishTypes();

            await repo.GetSubDishTypesList();

            return View(model);
        }
        //async or not??
        public async Task<IActionResult> PageData(IDataTablesRequest request)
        {
            var model = await repo.GetIngredientTypes();

            return Json(model);
        }

        // Add ingredients to the dish
        [HttpPost]
        public async Task<IActionResult> AddIngredientToDish(int IngredientTypeID, int DishTypeID)
        {
            Ingredient ingredient = new Ingredient();


            DishType P = await repo.GetDishTypeID(DishTypeID);

            if (P == null)
            {
                return NotFound();
            }

            IngredientType T = await repo.GetIngredientTypeID(IngredientTypeID);

            if (T == null)
            {
                return NotFound();
            }

            await repo.GetIngredients();

            ingredient.Type = T;

            if (P.DefaultIngredients == null)
            {
                P.DefaultIngredients = new List<Ingredient>();
            }

            P.DefaultIngredients.Add(ingredient);

            repo.Save();

            return RedirectToAction("Edit", new { id = DishTypeID });
        }

        // Remove ingredients from the dish
        public IActionResult RemoveIngredient(int? DishTypeID, int? IngredientID)
        {
            if (IngredientID == null)
            {
                return NotFound();
            }

            if (DishTypeID == null)
            {
                return NotFound();
            }

            repo.RemoveIngredientID(IngredientID);
            repo.Save();

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

            var model = await repo.GetDishTypeID(id);
            if (model == null)
            {
                return NotFound();
            }
            await repo.GetSubDishTypesList();

            return View(model);
        }

        // GET: DishTypes/Create
        public async Task<IActionResult> Create()
        {
            repo.EnsureCreated();
            
            DishTypesViewModel model = new DishTypesViewModel
            {
                SubTypeList = await repo.GetSubDishTypesList()
            };

            return View(model);
        }

        // POST: DishTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dish, Dish.Name, Dish.Course, Ingredients, Dish.DishTypeID, SubTypeID, Dish.Recipe, Dish.Price")]DishTypesViewModel dishTypeViewModel)
        {
            //HEAD
            if (ModelState.IsValid)
            {
                // Create new Dishtype and assign properties
                DishType model = new DishType
                {
                    Name = dishTypeViewModel.Dish.Name,
                    Course = dishTypeViewModel.Dish.Course,
                    Recipe = dishTypeViewModel.Dish.Recipe,
                    Price = dishTypeViewModel.Dish.Price
                };

                var sdt = repo.GetSubDishTypeID(dishTypeViewModel);               


                if (sdt == null)
                {
                    return NotFound();
                }

                model.SubDishType = sdt;

                repo.InsertDishType(model);
                repo.Save();

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

            var dishType = await repo.GetDishTypeID(id);

            await repo.GetIngredients();

            if (dishType == null)
            {
                return NotFound();
            }

            var ingredientType = await repo.GetIngredientTypes();

            var subDishType = await repo.GetSubDishTypesList();

            dishTypesViewModel.Dish = dishType;
            dishTypesViewModel.Ingredients = ingredientType;
            dishTypesViewModel.SubTypeList = subDishType;

            return View("Edit", dishTypesViewModel);
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
                    repo.UpdateDish(dishType);
                    repo.Save();
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

            var model = await repo.GetDishTypeID(id);


            dishTypesViewModel.SubTypeList = await repo.GetSubDishTypesList();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: DishTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            repo.RemoveDish(id);
            repo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool DishTypeExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
