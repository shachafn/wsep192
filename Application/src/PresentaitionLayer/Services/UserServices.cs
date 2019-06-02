using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.Extensions.Logging;
using PresentaitionLayer.Models;

namespace PresentaitionLayer.Services
{
    public class UserServices 
    {
        IServiceFacade _serviceFacade;
        ILogger<UserServices> _logger;
        public UserServices(IServiceFacade serviceFacade, ILogger<UserServices> logger)
        {
            _serviceFacade = serviceFacade;
            _logger = logger;
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
