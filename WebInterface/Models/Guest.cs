using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        /// <summary>
        /// The age of the customer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group wo which this guest belongs.
        /// A guest should be created when he or she scans 
        /// a QR code. At that time the table is know, hence 
        /// the current group, hence a Guest can always 
        /// belongs to a specific group.
        /// </summary>
        public Group Group { get; set; }
    }
}
