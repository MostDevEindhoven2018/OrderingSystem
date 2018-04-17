using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class Group
    {
        /// <summary>
        /// The guests that belong to this group. 
        /// This should be the list of all guests that have this group set as their group.
        /// </summary>
        public List<Guest> CurrentGuests { get; set; }

        /// <summary>
        /// The table at which this group is seated.
        /// </summary>
        public Table Table { get; set; }

        /// <summary>
        /// Wether to hide the price or not
        /// </summary>
        public bool HidePrice { get; set; }
    }
}
