using DomainLayer;
using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Data.Entitites;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        public User Register(string username, string password)
        {
            return DomainLayer.Domains.UserDomain.Register(username, password);
        }
        public bool Login(string username, string password)
        {
            User guest = new User();
            return DomainLayer.Domains.UserDomain.Login(username, password);
        }

        public bool Logout(string username)
        {
            return DomainLayer.Domains.UserDomain.LogoutUser(username);
        }

        public Guid OpenShop(string username)
        {
            return DomainLayer.Domains.UserDomain.OpenShopForUser(username);
        }

        public bool PurchaseBag(string username)
        {
            return DomainLayer.Domains.UserDomain.PurchaseBag(username);
        }
    }
}
