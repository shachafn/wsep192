using System;
using System.Collections.Generic;

namespace DomainLayer
{
    public class ShopOwner
    {
        private static OwnersDictionary shopOwners = new OwnersDictionary();
        private User owner; // may be  a list of owners is needed
        private Shop shop;
        private List<ShopOwner> ownersAssigned;
        // according to our design : manager is an owner with limited actions availabe to him
       //rivate List<ShopOwner> managersAssigned; i dont think we  need it but need to talk about it
        private ManagingPrivileges privileges;

        public ShopOwner(User owner,Shop shop,bool manager=false)
        {
            this.owner = owner;
            this.shop = shop;
            ownersAssigned = new List<ShopOwner>();
            this.privileges = new ManagingPrivileges(manager);
            this.shop.AddOwner(owner);
        }
        /// <summary>
        /// get a shopOwner instance from the current shopOwners in the system
        /// </summary>
        /// <param name="user"></param>
        /// <param name="shop"></param>
        /// <returns> a relvant shopOwner if one exists , null otherwise</returns>
        public static ShopOwner GetShopOwner(User user, Shop shop)
        {
            bool exists = user.IsLogged() && shopOwners.HasUser(user.Username);
            if(!exists)
            {
                return null;
            }
            foreach(ShopOwner shopOnwer in shopOwners.ShopsByUsername(user.Username))
            {
                if(shopOnwer.shop.Equals(shop))
                {
                    return shopOnwer;
                }
            }
            return null;
        }

        /// <summary>
        /// assigns an manager with specific actions allowed to him
        /// </summary>
        /// <param name="newManager">the user to assign as manager</param>
        /// <param name="privileges">the actions allowed to this manager</param>
        /// <returns>false if the manager couldn't be assigned ' true otherwise</returns>
        public bool AddManager(User newManager,ManagingPrivileges privileges)
        {
            if (!this.privileges.HasPrivilege("addManager"))
            {
                return false;
            }
            ShopOwner newShopOwner = new ShopOwner(newManager, shop, true)
            {
                privileges = privileges
            };
            ownersAssigned.Add(newShopOwner);
            shopOwners.OwnersDictAdd(newManager.Username,newShopOwner);
            return true;
        }

        /// <summary>
        /// assigns a user as a shop owner , with all the actions available to him
        /// </summary>
        /// <param name="newManager">the user to assign</param>
        /// <returns>false if the manager couldn't be assigned ' true otherwise</returns>
        public bool AddOwner(User newManager)
        {
            if (!this.privileges.HasPrivilege("addOwner"))
            {
                return false;
            }
            ShopOwner newShopOwner = new ShopOwner(newManager, this.shop, false);
            ownersAssigned.Add(newShopOwner);
            shopOwners.OwnersDictAdd(newManager.Username, newShopOwner);
            return true;
        }
    
        public static void NewShopOwner(User owner , Shop shop)
        {
            ShopOwner newShopOwner = new ShopOwner(owner, shop, false);
            shopOwners.OwnersDictAdd(owner.Username,newShopOwner);
        }
        /// <summary>
        /// removes a manger or a owner you assigned
        /// </summary>
        /// <param name="toRemove"> the Shopowner you want to remove</param>
        /// <returns>a boolean value according to the wether this <"toRemove"> is assigned by this owner, and is not already removed </returns>
        public bool RemoveOwner(ShopOwner toRemove)
        {
            if (!this.privileges.HasPrivilege("RemoveOwner") || !ownersAssigned.Contains(toRemove))
            {
                return false;
            }
            //first remove all of the owners/managers assigned by this owner
            foreach (ShopOwner assigned in toRemove.ownersAssigned)
            {
                RemoveOwner(assigned);
            }
            ownersAssigned.Remove(toRemove);
            shopOwners.OwnersDictRemove(toRemove.owner.Username, toRemove);
            this.shop.RemoveOwner(toRemove.owner);
            toRemove.owner.RemoveShop(toRemove.shop);
            return true;
        }

        /// <summary>
        /// closes the shop owned by this shop owner 
        /// removing all owners , and also this shop owner . and invoking shop.close
        /// </summary>
        public bool CloseShop()
        {
            if (!this.privileges.HasPrivilege("CloseShop"))
            {
                return false;
            }
            foreach(ShopOwner owner in ownersAssigned)
            {
                RemoveOwner(owner);
            }
            this.shop.RemoveOwner(this.owner);
            this.shop.Close();
            shopOwners.OwnersDictRemove(this.owner.Username, this); // remove yourself from the list
            this.owner.RemoveShop(this.shop);
            return true;
        }

        public static void CleanDict() // a method for cleaning the dictionary, used for testing
        {
            shopOwners.CleanDict();
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "Owner: "+owner.ToString()+"\n Shop: "+shop.ToString();
        }

        public class ManagingPrivileges
        {
            private List<string> AllowedActions { get => AllowedActions; set => AllowedActions = value; }
            private bool _isManager; // if not a manager it is an owner , an owner has all the actions available 
            public ManagingPrivileges(List<string> allowedActions)
            {
                AllowedActions = allowedActions;
                _isManager = true;
            }
            public ManagingPrivileges(bool isManager)
            {
                AllowedActions = new List<string>();
                _isManager = isManager;
            }

            public bool AddAction(string action)
            {
                bool exsits = AllowedActions.Contains(action);
                if(!exsits)
                {
                    AllowedActions.Add(action);              
                }
                return !exsits;
            }

            public bool RemoveAction(string action)
            {
                bool exsits = AllowedActions.Contains(action);
                if (exsits)
                {
                    AllowedActions.Remove(action);
                }
                return exsits;
            }

            public bool HasPrivilege(string action)
            {
                return !_isManager || AllowedActions.Contains(action);
            }
        }

        private class OwnersDictionary
        {
            private Dictionary<string, List<ShopOwner>> shopOwners = new Dictionary<string, List<ShopOwner>>();// will hold all the current shopOwners

            public OwnersDictionary()
            {
                shopOwners = new Dictionary<string, List<ShopOwner>>();
            }

            public void OwnersDictRemove(string username, ShopOwner toRemove)
            {
                List<ShopOwner> ownedShops = shopOwners[username];
                if (ownedShops.Count == 1)
                {
                    shopOwners.Remove(username);
                }
                else
                {
                    ownedShops.Remove(toRemove);
                }
            }

            public void OwnersDictAdd(string username, ShopOwner newshopOwner)
            {
                if (shopOwners.ContainsKey(username))
                {
                    shopOwners[username].Add(newshopOwner);
                }
                else
                {
                    List<ShopOwner> userShops = new List<ShopOwner>
                    {
                        newshopOwner
                    };
                    shopOwners.Add(username, userShops);
                }
            }

            public List<ShopOwner> ShopsByUsername(string username)
            {
                return shopOwners[username];
            }
            public bool HasUser(string username)
            {
                return shopOwners.ContainsKey(username);
            }

            public void CleanDict() // a method for cleaning the dictionary
            {
                shopOwners = new Dictionary<string, List<ShopOwner>>();
            }
        }
    }

}