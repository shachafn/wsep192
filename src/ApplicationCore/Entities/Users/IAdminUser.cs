using System;

namespace ApplicationCore.Entities.Users
{
    public interface IAdminUser : IUser
    {
        bool RemoveUser(Guid userToRemoveGuid);

        bool ConnectToPaymentSystem();

        bool ConnectToSupplySystem();
    }
}
