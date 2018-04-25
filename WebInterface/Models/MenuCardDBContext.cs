using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class MenuCardDBContext : DbContext
    {
        public MenuCardDBContext(DbContextOptions<MenuCardDBContext> options) : base(options)
        {

        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<SubDishType> SubDishTypes { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
    }
}
