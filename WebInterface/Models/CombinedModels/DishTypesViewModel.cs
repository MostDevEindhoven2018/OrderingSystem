using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInterface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebInterface.ViewModel
{
    /// <summary>
    /// ViewModel used by the DishTypesController
    /// </summary>
    public class DishTypesViewModel 
    {
        public DishType Dish { get; set; }

        public List<IngredientType> Ingredients { get; set; }

        public List<SubDishType> SubTypeList { get; set; }

        public int SubTypeID { get; set; }
    }
}
