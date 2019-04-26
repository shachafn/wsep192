using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PressentaitionLayer.Models;
using ServiceLayer;

namespace PressentaitionLayer.Services
{
    public static class UserServices 
    {
        public static Task<(bool, UserModel)> ValidateUserCredentialsAsync(string username, string password) // logs the user in
        {
            var isValid = Program.Service.Login(new Guid(),username,password);
            var user = new UserModel();
            user.Id = Guid.Empty;
            user.Password = password;
            user.UserName = username;
            var result = (isValid, user);
            return Task.FromResult(result);
        }
    }
}
