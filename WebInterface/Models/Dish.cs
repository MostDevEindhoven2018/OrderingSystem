using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Dish
    {
        public int DishID { get; set; }
        //Name changed from course to DishOfDishType
        public virtual DishType Course { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
