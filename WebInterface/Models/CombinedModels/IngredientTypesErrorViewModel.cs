using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInterface.Models;

namespace WebInterface.ViewModel
{
    public class IngredientTypesErrorViewModel
    {
        public IngredientType IngredientType { get; set; }

        public string Error { get; set; }
    }
}
