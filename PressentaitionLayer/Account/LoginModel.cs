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
        public enum AccountType
        {
            Buyer,
            Seller,
            Admin
        }

        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }



        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("user type")]
        public AccountType UserType { get; set; } //the user type
    }
}
