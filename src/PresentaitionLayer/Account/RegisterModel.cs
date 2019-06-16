using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PresentaitionLayer.Account
{
    public class RegisterModel
    {
        [Required]
        [StringLength(10,MinimumLength = 3,ErrorMessage = "{0} must be at least {2} characters long")]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Password")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} must be at least {2} characters long")]
        //[RegularExpression("^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$")] add when we want more security
        public string Password { get; set; }

        [Required]
        [DisplayName("PasswordVal")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} must be at least {2} characters long")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string PasswordVal { get; set; }

        public string countryOfYou { get; set; }
    }
}
