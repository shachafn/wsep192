using DomainLayer;
using NUnit.Framework;
using System.Collections.Generic;
using DomainLayer.Data.Entitites;

namespace Tests
{
    [TestFixture]
    public class UserTest1
    {

        [Test]
        public void TestGuest()
        {
            User user = new User();
            Assert.False(user.IsLoggedIn);
        }
        [Test,Description("testing succesfull register")]
        public void TestRegister()
        {
            var user = DomainLayer.Domains.UserDomain.Register("meni","123456");
            Assert.NotNull(user);
            Assert.AreEqual("meni", user.Username);
            Assert.AreEqual(1, DomainLayer.Domains.UserDomain.GetUsersCount());
        }

        [Test, Description("testing register with the same user name and with short password")]
        public void TestRegisterTwice()
        {
            User user = DomainLayer.Domains.UserDomain.Register("meni", "123456");
            Assert.Null(user);
            User otherUser = new User("beni", "1234", false);
            Assert.Null(otherUser);
            Assert.AreEqual(1, DomainLayer.Domains.UserDomain.GetUsersCount());
        }
    }

    

    [TestFixture]
    public class UserTest2
    {
        User meni;
        string meniPass;
        User beni;
        string beniPass;
        [SetUp]
        public void Setup()
        {
            meni = DomainLayer.Domains.UserDomain.Register("meni", "123456");
            beni = DomainLayer.Domains.UserDomain.Register("beni", "123456");
            beniPass = "123456";
            meniPass = "123456";
        }

        [Test,Description("test login")]
        public void Test1()
        {
            Assert.True(DomainLayer.Domains.UserDomain.Login(meni.Username, meniPass));
            Assert.True(meni.IsLoggedIn);
            Assert.False(DomainLayer.Domains.UserDomain.Login(meni.Username,meniPass));
            Assert.False(DomainLayer.Domains.UserDomain.Login(beni.Username,"21314454"));
        }

        [Test, Description("test logout")]
        public void Test2()
        {
            DomainLayer.Domains.UserDomain.Login(meni.Username, meniPass);
            Assert.True(DomainLayer.Domains.UserDomain.LogoutUser(meni.Username));
            Assert.False(DomainLayer.Domains.UserDomain.LogoutUser(meni.Username));
            Assert.False(DomainLayer.Domains.UserDomain.LogoutUser(beni.Username));
        }

    }
}