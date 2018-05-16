﻿using System;
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
        
        /// <summary>
        /// Used by the client to retrieve the IngredientTypes.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A JSon representation of the IngredientTypes from teh database</returns>
        public async Task<IActionResult> PageData(IDataTablesRequest request)
        {
            var model = await repo.GetIngredientTypes();

            return Json(model);
        }

        // Add ingredients to the dish
        [HttpPost]
        public async Task<IActionResult> AddIngredientToDish(int IngredientTypeID, int DishTypeID)
        {
            // This is the dishtype where we need to add an ingredient to
            DishType dishType = await repo.GetDishTypeID(DishTypeID);

            if (dishType == null)
            {
                return NotFound();
            }

            IngredientType type = await repo.GetIngredientTypeID(IngredientTypeID);

            if (type == null)
            {
                return NotFound();
            }

            await repo.GetIngredients();

            Ingredient ingredient = new Ingredient
            {
                Type = type
            };

            if (dishType.DefaultIngredients == null)
            {
                dishType.DefaultIngredients = new List<Ingredient>();
            }

            dishType.DefaultIngredients.Add(ingredient);

            await repo.Save();

            return RedirectToAction("Edit", new { id = DishTypeID });
        }

        // Remove ingredients from the dish
        public async Task<IActionResult> RemoveIngredient(int? DishTypeID, int? IngredientID)
        {
            if (IngredientID == null || DishTypeID == null)
            {
                return NotFound();
            }

            repo.RemoveIngredient(IngredientID);
            await repo.Save();

            return RedirectToAction("Edit", new { id = DishTypeID });
        }

        public async Task<IActionResult> SaveQuantity (int DishTypeID, int IngredientID, int Quantity)
        {
            await repo.GetIngredientTypeID(IngredientID, Quantity);
            await repo.Save();

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
            var model2 = await repo.GetIngredientTypes();
            if (model == null)
            {
                return NotFound();
            }

            DishTypesViewModel a = new DishTypesViewModel() {

                Dish=model,
                Ingredients=model2
            };



            return View(a);
        }

        // GET: DishTypes/Create
        public async Task<IActionResult> Create()
        {
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

                SubDishType sdt = await repo.GetSubDishTypeID(dishTypeViewModel.SubTypeID);

                if (sdt == null)
                {
                    return NotFound();
                }

                model.SubDishType = sdt;
                repo.InsertDishType(model);
                await repo.Save();

                int id = model.DishTypeID;
                return RedirectToAction(nameof(Edit), new { id });
            }

            return View(dishTypeViewModel);
        }

        // GET: DishTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishType = await repo.GetDishTypeID(id);

            if (dishType == null)
            {
                return NotFound();
            }

            var ingredientType = await repo.GetIngredientTypes();
            var subDishType = await repo.GetSubDishTypesList();

            var dishTypesViewModel = new DishTypesViewModel
            {
                Dish = dishType,
                Ingredients = ingredientType,
                SubTypeList = subDishType
            };

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
                    await repo.Save();
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

            DishType model = await repo.GetDishTypeID(id);

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
            await repo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool DishTypeExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
