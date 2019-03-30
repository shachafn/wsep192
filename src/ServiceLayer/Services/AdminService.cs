using ServiceLayer.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;
using DomainLayer.External_Services;
using System.Linq;

namespace ServiceLayer.Services
{
    public class AdminService : IAdminService
    {
        public bool ConnectToPaymentSystem()
        {
            return ExternalServicesManager._paymentSystem.IsAvailable();
        }

        public bool ConnectToSupplySystem()
        {
            return ExternalServicesManager._supplySystem.IsAvailable();
        }

        public bool Initialize(string username = null, string password = null)
        {
            if (!ConnectToPaymentSystem())
                return false;
            if (!ConnectToSupplySystem())
                return false;
            if (!ExistsAdminUser() && username != null && password != null)
                return User.Register(username, password) != null;
            return true;
        }

        private bool ExistsAdminUser() => User.users.Any(keyValue => keyValue.Value.IsAdmin);


        public bool RemoveUser(string username)
        {
            var user = User.GetUserByUsername(username);
            if (user == null) return false;
            return user.RemoveUser(username);
        }
    }
}
