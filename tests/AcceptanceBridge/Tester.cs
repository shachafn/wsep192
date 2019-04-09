using NUnit.Framework;
using System;

namespace Tests
{

    public static class Tester
    {
        public static bool _groismanConnected = false;
        public static Guid _groismanShop = Guid.Empty;
        public static Guid galaxyGuid = Guid.Empty;

        public static void ExecuteAcceptanceTests()
        {
            AdminAT _adminAT;
            RegisteredBuyerAT _registerAT;
            ShopManagerAT _shopManagerAT;
            StoreOwnerAT _storeOwnerAT;
            UserAT _userAT;
            _adminAT = new AdminAT();
            _registerAT = new RegisteredBuyerAT();
            _shopManagerAT = new ShopManagerAT();
            _storeOwnerAT = new StoreOwnerAT();
            _userAT = new UserAT();
            _adminAT.RunAdminAT();
            _registerAT.RunRegisteredUserAT();
            _shopManagerAT.RunShopManagerAT();
            _storeOwnerAT.RunStoreOwnerAT();
            _userAT.RunUserAT();
        }
    }
}