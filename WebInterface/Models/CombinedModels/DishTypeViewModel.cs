using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebInterface.Models.CombinedModels
{
    public class DishTypeViewModel
    {        
        public List<DishType> DishTypes { get; set; }
        public List<SubDishType> SubDishTypes { get; set; }
    }
}
