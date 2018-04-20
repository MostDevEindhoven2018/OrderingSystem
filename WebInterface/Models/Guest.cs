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
        /// The code of a guest is used to ensure that 2 guests cannot use each others pages by guessing their ID
        /// </summary>
        public string Code { get; set; }

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
        public virtual Group Group { get; set; }
    }
}
