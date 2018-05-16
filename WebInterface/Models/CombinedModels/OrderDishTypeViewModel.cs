using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models.CombinedModels
{
    public class OrderDishTypeViewModel
    {
        public List<Dish> OrderDishes { get; set; }     
        public List<SubDishType> OrderSubDishType { get; set; }
        public List<Order> Orders { get; set; }
    }
}