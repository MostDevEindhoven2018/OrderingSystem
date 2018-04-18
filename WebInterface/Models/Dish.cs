using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Dish
    {
        public int DishID { get; set; }
        public DishType Type { get; private set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
