using DomainLayer;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class UserTest1
    {

        [Test]
        public void TestGuest()
        {
            User user = new User();
            Assert.False(user.IsLogged());
        }
        [Test,Description("testing succesfull register")]
        public void TestRegister()
        {
            User user = User.Register("meni","123456");
            Assert.NotNull(user);
            Assert.Equals("meni", user.Username);
            Assert.Equals(1, User.users.Count);
        }

        [Test, Description("testing register with the same user name and with short password")]
        public void TestRegisterTwice()
        {
            User user = User.Register("meni", "123456");
            Assert.Null(user);
            User otherUser = new User("beni", "1234");
            Assert.Null(otherUser);
            Assert.Equals(1, User.users.Count);
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
            User.users = new Dictionary<string, User>();
            meni = User.Register("meni", "123456");
            beni = User.Register("beni", "123456");
            beniPass = "123456";
            meniPass = "123456";
        }

        [Test,Description("test login")]
        public void Test1()
        {
            Assert.True(meni.Login(meni.Username, meniPass));
            Assert.True(meni.IsLogged());
            Assert.False(meni.Login(meni.Username,meniPass));
            Assert.False(beni.Login(beni.Username,"21314454"));
        }

        [Test, Description("test logout")]
        public void Test2()
        {
            meni.Login(meni.Username, meniPass);
            Assert.True(meni.Logout());
            Assert.False(meni.Logout());
            Assert.False(beni.Logout());
        }

    }
}