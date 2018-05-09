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


namespace WebInterface.Repositories
{
    public class DishTypeRepository
    {
        private readonly MenuCardDBContext _context;

        public DishTypeRepository(MenuCardDBContext context)
        {
            _context = context;
        }

        public Task<List<DishType>> GetDishes()
        {
            return _context.DishTypes.ToListAsync();            
        }

        public Task<List<SubDishType>> GetSubDishes()
        {
            return _context.SubDishTypes.ToListAsync();
        }

        public Task<List<IngredientType>> GetIngredients()
        {
            return _context.IngredientTypes.ToListAsync();
        }

        public Task<DishType> GetDishTypeID(int DishTypeID)
        {
            var P = _context.DishTypes.Where(x => x.DishTypeID == DishTypeID).FirstOrDefaultAsync();
            return P;
        }

        public Task<IngredientType> GetIngredientTypeID(int IngredientTypeID)
        {
            var id = _context.IngredientTypes.Where(x => x.IngredientTypeID == IngredientTypeID).FirstOrDefaultAsync();
            return id;
        }
    }
}
