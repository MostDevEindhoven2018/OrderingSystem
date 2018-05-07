using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class FinalizedOrder
    {
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public string Name { get; set; }
    }
}
