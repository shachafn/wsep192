using NUnit.Framework;
using DomainLayer.Facade;
using System;

namespace Tests
{
    [TestFixture]
    public class UserTest1
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;

        [Test,Description("testing succesfull register")]
        public void TestRegister()
        {
            var userGuid = facade.Register("meni","123456");
            Assert.False(userGuid.Equals(Guid.Empty)); 
        }

        [Test, Description("testing register with the same user name.")]
        public void TestRegisterTwice()
        {
            var userGuid = facade.Register("meni", "123456");
            Assert.True(userGuid.Equals(Guid.Empty));
        }

        [Test, Description("testing register with null values")]
        public void TestRegisterNull()
        {
            var userGuid = facade.Register(null, "123456");
            Assert.True(userGuid.Equals(Guid.Empty));
            userGuid = facade.Register("beni", null);
            Assert.True(userGuid.Equals(Guid.Empty));
        }

        [Test, Description("testing register with empty strings")]
        public void TestRegisterEmpty()
        {
            var userGuid = facade.Register("", "123456");
            Assert.True(userGuid.Equals(Guid.Empty));
            userGuid = facade.Register("ceni", "");
            Assert.True(userGuid.Equals(Guid.Empty));
        }
    }

    

    [TestFixture]
    public class UserTest2
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;

        Guid meniGuid;
        const string meniUsername = "meni";
        const string meniPassword = "123456";

        Guid beniGuid;
        const string beniUsername = "beni";
        const string beniPassword = "123456";

        [OneTimeSetUp]
        public void Setup()
        {
            meniGuid = facade.Register("meni", "123456");
            beniGuid = facade.Register("beni", "123456");
        }

        [Test,Description("Test Login")]
        public void Test1()
        {
            meniGuid = facade.Login(meniUsername, meniPassword);
            Assert.False(meniGuid.Equals(Guid.Empty)); // First login success

            meniGuid = facade.Login(meniUsername, meniPassword);
            Assert.True(meniGuid.Equals(Guid.Empty)); // Second login fail

            beniGuid = facade.Login(beniUsername, beniPassword);
            Assert.False(beniGuid.Equals(Guid.Empty)); // First login success
        }

        [Test, Description("Test Logout")]
        public void Test2()
        {
            Assert.True(facade.Logout(meniGuid)); // First logout success
            Assert.False(facade.Logout(meniGuid)); // Second logout fail
            Assert.True(facade.Logout(beniGuid)); // First logout success
        }

    }
}