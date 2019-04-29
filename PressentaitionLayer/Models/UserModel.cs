using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
