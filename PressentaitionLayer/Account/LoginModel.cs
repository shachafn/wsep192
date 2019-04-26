using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Account
{
    public class LoginModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }



        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("user type")]
        public string UserType { get; set; } //the user type
    }
}
