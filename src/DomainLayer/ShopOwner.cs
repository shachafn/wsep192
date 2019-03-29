using System;
using System.Collections.Generic;

namespace DomainLayer
{
    public class ShopOwner
    {
        private ownersDictionary shopOwners;
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
        }
        /// <summary>
        /// get a shopOwner instance from the current shopOwners in the system
        /// </summary>
        /// <param name="user"></param>
        /// <param name="shop"></param>
        /// <returns> a relvant shopOwner if one exists , null otherwise</returns>
        public ShopOwner GetShopOwner(User user, Shop shop)
        {
            bool exists = user.isLogged() && shopOwners.hasUser(user.username);
            if(!exists)
            {
                return null;
            }
            foreach(ShopOwner shopOnwer in shopOwners.shopsByUsername(user.username))
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
        public bool addManager(User newManager,ManagingPrivileges privileges)
        {
            if (!this.privileges.hasPrivilege("addManager"))
            {
                return false;
            }
            this.shop.addOwner(newManager);
            ShopOwner newShopOwner = new ShopOwner(newManager,this.shop,true);
            newShopOwner.privileges = privileges;
            ownersAssigned.Add(newShopOwner);
            shopOwners.ownersDictAdd(newManager.username,newShopOwner);
            return true;
        }

        /// <summary>
        /// assigns a user as a shop owner , with all the actions available to him
        /// </summary>
        /// <param name="newManager">the user to assign</param>
        /// <returns>false if the manager couldn't be assigned ' true otherwise</returns>
        public bool addOwner(User newManager)
        {
            if (!this.privileges.hasPrivilege("addOwner"))
            {
                return false;
            }
            this.shop.addOwner(newManager);
            ShopOwner newShopOwner = new ShopOwner(newManager, this.shop, false);
            ownersAssigned.Add(newShopOwner);
            shopOwners.ownersDictAdd(newManager.username, newShopOwner);
            return true;
        }

        /// <summary>
        /// removes a manger or a owner you assigned
        /// </summary>
        /// <param name="toRemove"> the Shopowner you want to remove</param>
        /// <returns>a boolean value according to the wether this <"toRemove"> is assigned by this owner, and is not already removed </returns>
        public bool removeOwner(ShopOwner toRemove)
        {
            if (!this.privileges.hasPrivilege("addOwner")|| !ownersAssigned.Contains(toRemove))
            {
                return false;
            }
            //first remove all of the owners/managers assigned by this owner
            foreach (ShopOwner assigned in toRemove.ownersAssigned)
            {
                removeOwner(assigned);
            }
            ownersAssigned.Remove(toRemove);
            shopOwners.ownersDictRemove(toRemove.owner.username, toRemove);
            this.shop.removeOwner(toRemove.owner);
            toRemove.owner.removeShop(toRemove.shop);
            return true;
        }

        /// <summary>
        /// closes the shop owned by this shop owner 
        /// removing all owners , and also this shop owner . and invoking shop.close
        /// </summary>
        public void closeShop()
        {
            if (!this.privileges.hasPrivilege("addOwner"))
            {

            }
            foreach(ShopOwner owner in ownersAssigned)
            {
                removeOwner(owner);
            }
            this.shop.removeOwner(this.owner);
            this.shop.close();
            shopOwners.ownersDictRemove(this.owner.username, this); // remove yourself from the list
            this.owner.removeShop(this.shop);
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "";
        }

        public class ManagingPrivileges
        {
            List<string> allowedActions { get => allowedActions; set => allowedActions = value; }
            bool isManager; // if not a manager it is an owner , an owner has all the actions available 
            public ManagingPrivileges(List<string> allowedActions)
            {
                this.allowedActions = allowedActions;
                this.isManager = true;
            }
            public ManagingPrivileges(bool isManager)
            {
                this.allowedActions = new List<string>();
                this.isManager = isManager;
            }

            public bool addAction(string action)
            {
                bool exsits = allowedActions.Contains(action);
                if(!exsits)
                {
                    allowedActions.Add(action);              
                }
                return !exsits;
            }

            public bool removeAction(string action)
            {
                bool exsits = allowedActions.Contains(action);
                if (exsits)
                {
                    allowedActions.Remove(action);
                }
                return exsits;
            }

            public bool hasPrivilege(string action)
            {

                return !isManager || allowedActions.Contains(action);
            }
        }

        private class ownersDictionary
        {
            private Dictionary<string, List<ShopOwner>> shopOwners = new Dictionary<string, List<ShopOwner>>();// will hold all the current shopOwners

            public ownersDictionary()
            {
                shopOwners = new Dictionary<string, List<ShopOwner>>();
            }

            public void ownersDictRemove(string username, ShopOwner toRemove)
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

            public void ownersDictAdd(string username, ShopOwner newshopOwner)
            {
                if (shopOwners.ContainsKey(username))
                {
                    shopOwners[username].Add(newshopOwner);
                }
                else
                {
                    List<ShopOwner> userShops = new List<ShopOwner>();
                    userShops.Add(newshopOwner);
                    shopOwners.Add(username, userShops);
                }
            }

            public List<ShopOwner> shopsByUsername(string username)
            {
                return shopOwners[username];
            }
            public bool hasUser(string username)
            {
                return shopOwners.ContainsKey(username);
            }
        }
    }

}