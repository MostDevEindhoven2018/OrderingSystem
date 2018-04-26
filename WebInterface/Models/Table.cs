using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Table
    {
        
        public int TableID { get; set; }
        public virtual Group CurrentGroup { get; set; }
        public int Capacity { get; set; }
    }
}
