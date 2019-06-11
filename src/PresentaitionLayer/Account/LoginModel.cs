using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PresentaitionLayer.Account
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
