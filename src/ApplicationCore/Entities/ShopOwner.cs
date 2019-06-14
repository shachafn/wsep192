using System;
using System.Collections.Generic;

namespace ApplicationCore.Entitites
{
    public class ShopOwner
    {
        public Guid OwnerGuid { get; set; }

        public Guid AppointerGuid { get; set; } // Guid.Empty for the creator of the shop.

        public Guid ShopGuid { get; set; }

        public enum PrivilegeEnum { ManageProducts = 0, ManageShopState = 1, ManagePolicies = 2, AppointManagers = 3 }

        public ICollection<PrivilegeEnum> Privileges { get; set; }

        public ShopOwner(Guid ownerGuid, Guid appointerGuid, Guid shopGuid, List<bool> privileges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = appointerGuid;
            ShopGuid = shopGuid;
            Privileges = new List<PrivilegeEnum>();
            if (privileges == null) return;
            Array values = Enum.GetValues(typeof(PrivilegeEnum));
            for (int i = 0; i < Enum.GetNames(typeof(PrivilegeEnum)).Length; i++)
            {
                if (privileges[i])
                {
                    Privileges.Add((PrivilegeEnum)values.GetValue(i));
                }
            }
        }

        public ShopOwner(Guid ownerGuid, Guid shopGuid, IList<bool> privileges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = Guid.Empty;
            ShopGuid = shopGuid;
            Privileges = new List<PrivilegeEnum>();
            if (privileges == null) return;
            Array values = Enum.GetValues(typeof(PrivilegeEnum));
            for (int i = 0; i < Enum.GetNames(typeof(PrivilegeEnum)).Length; i++)
            {
                if (privileges[i])
                {
                    Privileges.Add((PrivilegeEnum)values.GetValue(i));
                }
            }
        }
    }
}
