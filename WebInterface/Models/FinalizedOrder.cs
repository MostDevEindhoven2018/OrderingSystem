using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class FinalizedOrder
    {
        public int Order { get; set; }
        public int Table { get; set; }

        public Table finalizedTables { get; set; }
        public virtual ICollection<Dish> finalizedDish { get; set; }
        public string DishName { get; set; }
        public int Quantity { get; set; }

    }
}
