using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ServiceLayer;
using PressentaitionLayer.Models;

namespace PressentaitionLayer.Services
{
    public class UserServices 
    {
        IServiceFacade _serviceFacade;

        public UserServices(IServiceFacade serviceFacade)
        {
            _serviceFacade = serviceFacade;
        }
        public Task<(bool, UserModel)> ValidateUserCredentialsAsync(string username, string password,string type,Guid guid) // logs the user in
        {
            var isValid = _serviceFacade.Login(guid,username,password);
            var user = new UserModel();
            user.Id = Guid.Empty;
            user.Password = password;
            user.UserName = username;
            user.UserType = type;
            var result = (isValid, user);
            return Task.FromResult(result);
        }

        public Task<(bool, UserModel)> ValidateUserRegisterAsync(string username, string password, Guid guid) // logs the user in
        {
            var isValid = _serviceFacade.Register(guid, username, password)!=Guid.Empty;
            var user = new UserModel();
            user.Id = Guid.Empty;
            user.Password = password;
            user.UserName = username;
            var result = (isValid, user);
            return Task.FromResult(result);
        }
    }
}
