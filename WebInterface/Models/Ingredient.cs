using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Ingredient
    {
        public IngredientType Type { get; private set; }
        public double Quantity { get; set; }
    }
}
