using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebInterface.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        /// <summary>
        /// Dishes added to the Order, but not send to the kitchen
        /// </summary>


        public DishType DishOrder { get; set; }
        public int quantity {get;set;}

        public virtual ICollection<Dish> Selected { get; set; }

        /// <summary>
        /// Dishes send to the kitchen. The guest cannot change these anymore.
        /// </summary>
        public virtual ICollection<Dish> Finalized { get; set; }

        /// <summary>
        /// The owner of an Order adds items to the Selected list, and then finalizes them.
        /// All Dishes of a single order have been selected by a single Guest. The dishes 
        /// might be intended for different Guest.
        /// </summary>
        public Guest Owner { get; set; }
    }
}
