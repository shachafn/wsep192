using DomainLayer;
using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        public User Register(string username, string password, out string ErrorMessage)
        {
            ErrorMessage = "";
            return User.Register(username, password);
        }
        public bool Login(string username, string password)
        {
            User guest = new User();
            return guest.Login(username, password);
        }

        public bool Logout(string username)
        {
            var user = User.GetUserByUsername(username);
            if (user == null) return false;
            return user.Logout();
        }

        public Guid OpenShop(string username, out string errorMessage)
        {
            var user = User.GetUserByUsername(username);
            if (user == null)
            {
                errorMessage = $"No such user with username {username}";
                return Guid.Empty;
            }
            errorMessage = "";
            return user.OpenShop();
        }

        public bool PurchaseBag(string username)
        {
            var user = User.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }
            user.PurchaseBag();
            return true;
        }
    }
}
