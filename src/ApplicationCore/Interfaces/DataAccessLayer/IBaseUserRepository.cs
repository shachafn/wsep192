using ApplicationCore.Entities.Users;
using System;

namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IBaseUserRepository : IRepository<BaseUser>
    {
        string GetUsername(Guid guid);

        BaseUser GetByUsername(string username);
        Guid GetUserGuidByUsername(string username);
        bool IsUserExistsByGuid(Guid guid);
        bool IsUserExistsByUsername(string username);
    }
}