using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInterface.Models;

namespace WebInterface.ViewModel
{
    public class DishTypesViewModel 
    {
        //internal readonly string Name;

        public DishType Dish { get; set; }

        public List<IngredientType> Ingredients { get; set; }

    }
}
