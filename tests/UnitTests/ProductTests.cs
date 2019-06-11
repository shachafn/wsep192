namespace UnitTests
{
    /*
    [TestFixture]
    public class ProductTests
    {
        User loggedInUser;
        User notLoggedInUser;
        Product football;

        [SetUp]
        public void Setup()
        {
            UserDomain.Register("meni", "123456");
            notLoggedInUser = UserDomain.Register("beni", "123456");
            string pass = "123456";
            UserDomain.Login(loggedInUser.Username, pass);
            football = new Product("Football", "Sports");
        }

        [Test, Description("Test Product Init")]
        public void TestProductInit()
        {
            Assert.AreEqual("Football", football.Name);
            Assert.AreEqual("Sports", football.Category);
            Assert.AreEqual(0, football.Rate);

        }
        
        [Test, Description("Test RateProduct")]
        public void TestRateProduct()
        {
            Assert.False(football.RateProduct(notLoggedInUser, 5));
            Assert.False(football.RateProduct(loggedInUser, 4));//Didn't buy the product
            Assert.AreEqual(0, football.Rate);

        }
        
    }
    */
}