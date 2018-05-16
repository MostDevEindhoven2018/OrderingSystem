using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;

namespace WebInterface.Models
{
    public class QRGeneration
    {
        // This property will hold a table, selected by user
        [Required]
        [Display(Name = "Table Number")]
        public string Table_Number { get; set; }

        // This property will hold all available tables for selection
        public IEnumerable<SelectListItem> Tables { get; set; }
    }
}
