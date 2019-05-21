using System;

namespace PressentaitionLayer.Models
{

    public class UserModel
    {
        public Guid Id { get; set; } //users guid
        public string Password { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; } //the user type
    }
}
