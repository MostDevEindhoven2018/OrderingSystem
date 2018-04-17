using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Group
    {
        public List<Guest> CurrentGuests { get; set; }
        public Table Table { get; set; }
    }
}
