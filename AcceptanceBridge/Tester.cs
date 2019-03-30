using NUnit.Framework;

namespace Tests
{

    public class Tester
    {
        private AdminAT _adminAT;
        private RegisteredBuyerAT _registerAT;
        private ShopManagerAT _shopManagerAT;
        private StoreOwnerAT _storeOwnerAT;
        private UserAT _userAT;
        [SetUp]
        public void Setup()
        {
            _adminAT = new AdminAT();
            _registerAT = new RegisteredBuyerAT();
            _shopManagerAT = new ShopManagerAT();
            _storeOwnerAT = new StoreOwnerAT();
            _userAT = new UserAT();
        }

        
        public void ExecuteAcceptanceTests()
        {
            _adminAT.RunAdminAT();
            _registerAT.RunRegisteredUserAT();
            _shopManagerAT.RunShopManagerAT();
            _storeOwnerAT.RunStoreOwnerAT();
            _userAT.RunUserAT();
        }
    }
}