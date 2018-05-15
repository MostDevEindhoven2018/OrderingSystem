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
        public Task CreateDB;

        public DishTypeRepository(MenuCardDBContext context)
        {
            _context = context;
            CreateDB = EnsureCreated();
        }        

        public async Task<List<DishType>> GetDishTypes()
        {
            return await _context.DishTypes.ToListAsync();            
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
            var id = _context.DishTypes.Include(x => x.DefaultIngredients).Include(x => x.SubDishType).Where(x => x.DishTypeID == DishTypeID).FirstOrDefaultAsync();
            return id;
        }

        public async Task<SubDishType> GetSubDishTypeID(DishTypesViewModel dishTypesViewModel)
        {
            DbSet<SubDishType> subDishTypes = GetSubDishTypes();
            var sdt = subDishTypes.Where(s => dishTypesViewModel.SubTypeID == s.SubDishTypeID).FirstOrDefaultAsync();
            return await sdt;
        }


        public async Task<Ingredient> GetIngredientTypeID(int? IngredientID, int Quantity)
        {
            var ingredientQuery = _context.Ingredients.Where(x => x.IngredientID == IngredientID);
            Ingredient ingredient = await ingredientQuery.FirstOrDefaultAsync();
            ingredient.Quantity = Quantity;
            return ingredient;
        }

        public void InsertDishType(DishType entity)
        {
            _context.Add(entity);
        }

        public void RemoveIngredient(int? IngredientID)
        {
            var ingredientID = _context.Ingredients.SingleOrDefault(x => x.IngredientID == IngredientID);
            _context.Ingredients.Remove(ingredientID);
        }

        public void RemoveDish(int? id)
        {
            DishType dishType = _context.DishTypes.Include(x => x.DefaultIngredients).Include(x => x.SubDishType).Where(x => x.DishTypeID == id).FirstOrDefault();
            ICollection<Ingredient> ingredients = dishType.DefaultIngredients;
            dishType.DefaultIngredients = null;
            _context.Update(dishType);

            foreach (Ingredient i in ingredients)
            {
                _context.Ingredients.Remove(i);
            }

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

        private Task<bool> EnsureCreated()
        {
            return _context.Database.EnsureCreatedAsync();
        }

        public void UpdateDish(DishTypesViewModel dishType)
        {
            _context.Update(dishType.Dish);
            _context.Update(dishType.Ingredients);
        }

        public bool Exists(int? id)
        {
            return _context.DishTypes.Any(e => e.DishTypeID == id);
        }
    }
}
