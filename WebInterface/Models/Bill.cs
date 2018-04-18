using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Bill
    {
        public int BillID { get; set; }
        public Guest Payer { get; set; }
    }
}
