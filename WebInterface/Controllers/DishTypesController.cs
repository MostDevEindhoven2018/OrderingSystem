using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;
using WebInterface.ViewModel;

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
            return View(await _context.DishTypes.ToListAsync());
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

            return View(dishType);
        }

        // GET: DishTypes/Create
        public IActionResult Create()
        {
            _context.Database.EnsureCreated();
            CreateDishTypeModel createDishType = new CreateDishTypeModel();
            var ListOfSubDishTypes = _context.SubDishTypes.Select(s => s.SubDishTypeID);
            //List<int> query = ListOfSubDishTypes.ToList();
            
            createDishType.SubTypeList = ListOfSubDishTypes.ToList();

            return View();
        }

        // POST: DishTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishTypeID,Course,Name,SubTypeID")] DishTypesViewModel createDishTypeViewModel)
        {
            //HEAD
            if (ModelState.IsValid)
            {
                DishType dishType = new DishType();
                dishType.Name = createDishTypeViewModel.Dish.Name;
                dishType.Course = createDishTypeViewModel.Dish.Course;

                ////_context.SubDishTypes.ToList().Select()


                //DbSet<SubDishType> subDishTypes = _context.SubDishTypes;
                //var query = subDishTypes.Where(s => createDishTypeModel.SubTypeID == s.SubDishTypeID);
                //SubDishType sdt = query.FirstOrDefault();

                //if (sdt==null)
                //{
                //    return NotFound();
                //}

                //dishType.SubType = sdt;

                _context.Add(dishType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //if (ModelState.IsValid)
            //{
            //    DishType dishType = new DishType();
            //    dishType.Name = createDishTypeModel.Name;
            //    dishType.Course = createDishTypeModel.Course;
            //    _context.SubDishTypes.ToList().Select(sdt => new { sdt.SubDishTypeID }).SingleOrDefault(sdt => sdt.SubDishTypeID == subDishTypeID);

            //    if (id == null)
            //    {
            //        return NotFound();
            //    }

            //    var subDishType = await _context.SubDishTypes.SingleOrDefaultAsync(m => m.SubDishTypeID == id);

            //    if (subDishType == null)
            //    {
            //        return NotFound();
            //    }
            //    _context.Add(dishType);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            // Make the ingredient page work and tried to make the categories in the dishes work

            return View(createDishTypeViewModel);
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

            dishTypesViewModel.Dish = dishType;
            dishTypesViewModel.Ingredients = ingredientType;

            return View(dishTypesViewModel);
        }

        // POST: DishTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishTypeID,Course,Name")] DishTypesViewModel dishType)
        {
            if (id != dishType.Dish.DishTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishType);
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
