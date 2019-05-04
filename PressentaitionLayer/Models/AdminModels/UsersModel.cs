using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Models.AdminModels
{
    public class UsersModel
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
    }
}
