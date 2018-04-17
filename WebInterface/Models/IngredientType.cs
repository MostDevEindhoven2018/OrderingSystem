using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class IngredientType
    {
        /// <summary>
        /// User readable name of this ingredient type.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The quentity of this ingredient type is expressed in this unit.
        /// Examples: "gram", "liter", "piece"
        /// </summary>
        public string UnitSingular { get; private set; }

        /// <summary>
        /// See UnitSingular for details. This is simply the plural version.
        /// </summary>
        public string UnitPlural { get; set; }
    }
}
