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
        public static Task<(bool, UserModel)> ValidateUserCredentialsAsync(string username, string password,string type,Guid guid) // logs the user in
        {
            var isValid = Program.Service.Login(guid,username,password);
            var user = new UserModel();
            user.Id = Guid.Empty;
            user.Password = password;
            user.UserName = username;
            user.UserType = type;
            var result = (isValid, user);
            return Task.FromResult(result);
        }

        public static Task<(bool, UserModel)> ValidateUserRegisterAsync(string username, string password, Guid guid) // logs the user in
        {
            var isValid = Program.Service.Register(guid, username, password)!=Guid.Empty;
            var user = new UserModel();
            user.Id = Guid.Empty;
            user.Password = password;
            user.UserName = username;
            var result = (isValid, user);
            return Task.FromResult(result);
        }
    }
}
