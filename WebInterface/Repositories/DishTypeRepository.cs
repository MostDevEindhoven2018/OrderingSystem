using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebInterface.Models;
using WebInterface.ViewModel;



namespace WebInterface.Repositories
{
    public class DishTypeRepository
    {
        private readonly MenuCardDBContext _context;

        public DishTypeRepository(MenuCardDBContext context)
        {
            _context = context;
        }

        public Task<List<DishType>> GetDishTypes()
        {
            return _context.DishTypes.ToListAsync();            
        }

        public Task<List<SubDishType>> GetSubDishTypesList()
        {
            return _context.SubDishTypes.ToListAsync();
        }

        public DbSet<SubDishType> GetSubDishTypes()
        {
            return _context.SubDishTypes;
        }

        public Task<List<Ingredient>> GetIngredients()
        {
            return _context.Ingredients.ToListAsync();
        }
        public Task<List<IngredientType>> GetIngredientTypes()
        {
            return _context.IngredientTypes.ToListAsync();
        }

        public Task<DishType> GetDishTypeID(int? DishTypeID)
        {
            var id = _context.DishTypes.Where(x => x.DishTypeID == DishTypeID).FirstOrDefaultAsync();
            return id;
        }

        public SubDishType GetSubDishTypeID(DishTypesViewModel dishTypesViewModel)
        {
            DbSet<SubDishType> subDishTypes = GetSubDishTypes();
            var sdt = subDishTypes.Where(s => dishTypesViewModel.SubTypeID == s.SubDishTypeID).FirstOrDefault();
            return sdt;
        }

        public void InsertDishType(DishType entity)
        {
            _context.Add(entity);
        }

        public void RemoveIngredientID(int? IngredientID)
        {
            var ingredientID = _context.Ingredients.SingleOrDefault(x => x.IngredientID == IngredientID);
            _context.Ingredients.Remove(ingredientID);
        }

        public void RemoveDish(int? id)
        {
            var dishType = _context.DishTypes.SingleOrDefault(x => x.DishTypeID == id);

            _context.DishTypes.Remove(dishType);
        }

        public Task<IngredientType> GetIngredientTypeID(int? IngredientTypeID)
        {
            var id = _context.IngredientTypes.Where(x => x.IngredientTypeID == IngredientTypeID).FirstOrDefaultAsync();
            return id;
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }

        public void EnsureCreated()
        {
            _context.Database.EnsureCreated();
        }

        public void UpdateDish(DishTypesViewModel dishType)
        {
            _context.Update(dishType.Dish);
        }

        public bool Exists(int? id)
        {
            return _context.DishTypes.Any(e => e.DishTypeID == id);
        }
    }
}
