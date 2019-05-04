using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressentaitionLayer.Models.AdminModels
{
    public class AdminUsersDisplayModel
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
    }
}
