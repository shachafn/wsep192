using NUnit.Framework;
using System;
using ATBridge;
namespace Tests
{

    public static class Tester
    {
        public static bool _groismanConnected = false;
        public static Guid _groismanShop = Guid.Empty;
        public static Guid galaxyGuid = Guid.Empty;
        public static Guid GroismanGuid = Guid.Empty;
        public static readonly Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");
        public static ProxyBridge PBridge = new ProxyBridge();
        public static void ExecuteAcceptanceTests()
        {
            
            //RegisteredBuyerAT _registerAT;
            //ShopManagerAT _shopManagerAT;
            //StoreOwnerAT _storeOwnerAT;
            //_registerAT = new RegisteredBuyerAT();
            //_shopManagerAT = new ShopManagerAT();
            //_storeOwnerAT = new StoreOwnerAT();
            AdminAT.RunAdminAT();
            //_registerAT.RunRegisteredUserAT();
            //_shopManagerAT.RunShopManagerAT();
            //_storeOwnerAT.RunStoreOwnerAT();
            UserAT.RunUserAT();
        }
    }
}