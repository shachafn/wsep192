namespace IntegrationTests
{
    /*
    [TestFixture]
    public class UserCartProductShopTests
    {
        User loggedInUser;
        User loggedInUser1;
        User notLoggedInUser;
        Product football;
        Product chair;
        Shop shop;

        [SetUp]
        public void Setup()
        {
            loggedInUser = DomainLayer.Domains.UserDomain.Register("meni", "123456");
            loggedInUser1 = DomainLayer.Domains.UserDomain.Register("reni", "123456");
            notLoggedInUser = DomainLayer.Domains.UserDomain.Register("beni", "123456");
            shop = new Shop();
            ShopOwner owner = new ShopOwner(loggedInUser, shop);
            string pass = "123456";
            DomainLayer.Domains.UserDomain.Login(loggedInUser.Username, pass);
            DomainLayer.Domains.UserDomain.Login(loggedInUser1.Username, pass);
            football = new Product("Football", "Sports");
            chair = new Product("Chair", "Furniture");
            shop.AddProduct(football, 49.90, 4);
            shop.AddProduct(chair, 399.99, 2);
        }
        [Test, Description("Test RemoveProduct")]
        public void TestRemoveProduct()
        {
            shop.RemoveProduct(chair);
            Assert.Null(shop.SearchProduct(chair));
            Assert.NotNull(shop.SearchProduct(football));
        }
        [Test, Description("Test PurchaseCart and RateShop")]
        public void TestPurchaseCartRateShop()
        {
            Assert.False(shop.RateShop(notLoggedInUser, 5));
            Assert.False(shop.RateShop(loggedInUser, 4)); //Didn't buy a product from the shop

            ShoppingCart shoppingCart = new ShoppingCart(shop);
            shoppingCart.AddProduct(shop.SearchProduct(football));
            shoppingCart.PurchaseCart(loggedInUser);
            Assert.True(shop.RateShop(loggedInUser, 5));
            Assert.AreEqual(5.0, shop.Rate);

            ShoppingCart shoppingCart1 = new ShoppingCart(shop);
            shoppingCart1.AddProduct(shop.SearchProduct(chair));
            shoppingCart1.PurchaseCart(loggedInUser1);
            Assert.True(shop.RateShop(loggedInUser1, 4));
            Assert.AreEqual(4.5, shop.Rate);
        }

        [Test, Description("Test PurchaseCart and RateProduct")]
        public void TestPurchaseCartRateProduct()
        {
            Assert.False(football.RateProduct(notLoggedInUser, 5));
            Assert.False(football.RateProduct(loggedInUser, 4)); //Didn't buy a product from the shop

            ShoppingCart shoppingCart = new ShoppingCart(shop);
            ShopProduct footballToBuy = shop.SearchProduct(football).Clone();
            footballToBuy.Quantity = 2;
            shoppingCart.AddProduct(footballToBuy);
            Assert.True(shoppingCart.PurchaseCart(loggedInUser));
            Assert.True(football.RateProduct(loggedInUser, 5));
            Assert.AreEqual(5.0, football.Rate);

            ShoppingCart shoppingCart1 = new ShoppingCart(shop);
            footballToBuy.Quantity = 1;
            shoppingCart1.AddProduct(footballToBuy);
            Assert.True(shoppingCart1.PurchaseCart(loggedInUser1));
            Assert.True(football.RateProduct(loggedInUser1, 4));
            Assert.AreEqual(4.5, football.Rate);

            ShoppingCart shoppingCart2 = new ShoppingCart(shop);
            footballToBuy.Quantity = 2;
            shoppingCart2.AddProduct(footballToBuy);
            Assert.False(shoppingCart2.PurchaseCart(loggedInUser1));//Exceeded the quantity available
        }

        [Test, Description("Test SendMessage")]
        public void TestSendMessage()
        {
            string shouldNotArrive = "My message should not arrive";
            string shouldArrive = "My Message should arrive";
            shop.SendMessage(notLoggedInUser, shouldNotArrive);
            Assert.AreEqual(0, shop.Messages.Count);
            shop.SendMessage(loggedInUser, shouldArrive);
            Assert.AreEqual(loggedInUser, shop.Messages[0].Item1);
            Assert.AreEqual(shouldArrive, shop.Messages[0].Item2);

        }
    }
    */
}
