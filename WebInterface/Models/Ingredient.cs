using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public virtual IngredientType Type { get; private set; }

        /// <summary>
        /// This quantity is expressed in units defined by the type of this ingredient.
        /// </summary>
        public double Quantity { get; set; }
    }
}
