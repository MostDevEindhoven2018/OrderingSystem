using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebInterface.Models.CombinedModels
{
    /// <summary>
    /// View Model used by the MenuCardController
    /// </summary>
    public class DishTypeViewModel
    {        
        public List<DishType> DishTypes { get; set; }
        public List<SubDishType> SubDishTypes { get; set; }
        public Dictionary<DishType, int> QuantityDictionary { get; set; }
    }
}
