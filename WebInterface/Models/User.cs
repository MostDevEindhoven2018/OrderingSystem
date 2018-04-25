using System;
using System.ComponentModel.DataAnnotations;
using WebInterface.Models;
using WebInterface.Controllers;

namespace WebInterface.Models
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