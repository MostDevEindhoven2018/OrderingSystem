using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    public class DishType
    {
        [Required]
        public string Name { get; set; }

        public int DishTypeID { get; set; }

        public virtual CourseType Course { get; set; }

        
        public SubDishType SubDishType { get; set; }

        [Required]
        [RegularExpression("([0-9 .]{1,9})", ErrorMessage = "This Field only accepts numbers and must not exceed 9 characters")]
        public double? Price { get; set; }

        [Required]
        public string Recipe { get; set; }

        public ICollection<Ingredient> DefaultIngredients { get; set; }
    }

    public enum CourseType
    {
        Unavailable,
        DRINK,
        STARTER,
        MAINCOURSE,
        DESSERT
    }
}
