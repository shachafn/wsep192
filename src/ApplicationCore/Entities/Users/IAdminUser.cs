using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Users
{
    public interface IAdminUser : IUser
    {
        bool RemoveUser(Guid userToRemoveGuid);

        bool ConnectToPaymentSystem();

        bool ConnectToSupplySystem();
    }
}
