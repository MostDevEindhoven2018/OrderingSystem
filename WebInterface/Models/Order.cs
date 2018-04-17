using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Order
    {
        public List<Dish> Selected { get; set; }
        public List<Dish> Finalized { get; set; }
        public Guest Payer { get; set; }
    }
}
