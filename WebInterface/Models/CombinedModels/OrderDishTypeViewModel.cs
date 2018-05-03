using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models.CombinedModels
{
    public class OrderDishTypeViewModel
    {
        public List<DishType> orderDishType { get; set; }
        public List<Order> orders { get; set; }
    }
}