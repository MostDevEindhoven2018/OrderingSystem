using System;
using System.ComponentModel.DataAnnotations;
using UserCustimizationMenu.Models;
using UserCustimizationMenu.Controllers;

namespace UserCustimizationMenu.Models
{
    public class User
    {
        public int UserID { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "This field is required!")]
        public string UserName { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "This field is required!")]
        public string Password { get; set; }


        public User()
        {
            
        }
    }
}