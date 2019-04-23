using NUnit.Framework;
using DomainLayer.Facade;
using System;
using DomainLayer.ExposedClasses;
using DomainLayer.Domains;
using DomainLayer.Data;
using System.Linq;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Exceptions;

namespace Tests
{
    
    [TestFixture]
    public class UserDomainTests
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;
        UserDomain userDomain = UserDomain.Instance;

        UserIdentifier _idnetifier = default(UserIdentifier);
        const string _adminUsername = "admin";
        const string _adminPassword = "000000";

        [SetUp]
        public void SetUp()
        {
            _idnetifier = new UserIdentifier(Guid.NewGuid(), true);
            facade.Initialize(_idnetifier, _adminUsername, _adminPassword);
        }

        [TearDown]
        public void TearDown()
        {
            facade.ClearSystem();
        }

        #region Register
        const string registerName = "GROISMAN";
        const string registerPassword = "pass";
        Guid registerGuid = Guid.Empty;

        [Test,Description("testing succesfull register")]
        public void TestRegister()
        {
            registerGuid = userDomain.Register(registerName, registerPassword, false);
            var addedGuid = DomainData.RegisteredUsersCollection.Any(r => r.Guid.Equals(registerGuid));
            Assert.IsTrue(addedGuid);
            var addedUsername = DomainData.RegisteredUsersCollection.Any(r => r.Username.Equals(registerName.ToLower()));
            Assert.IsTrue(addedUsername);
        }
        
        [Test, Description("testing register with the same user name.")]
        public void TestRegisterTwice()
        {
            TestRegister();
            var res = userDomain.Register(registerName, registerPassword, false);
            Assert.AreEqual(Guid.Empty, res);
        }
        #endregion

        #region Login
        [Test, Description("Test Login")]
        public void TestLogin()
        {
            TestRegister();
            userDomain.Login(registerName,registerPassword);
            var added = DomainData.LoggedInUsersEntityCollection.FirstOrDefault(r => r.Guid.Equals(registerGuid));
            Assert.AreNotEqual(null, added);
        }
        #endregion

        #region Logout

        [Test, Description("Test Logout")]
        public void TestLogout()
        {
            TestLogin();
            userDomain.LogoutUser(new UserIdentifier(registerGuid, false));
            var added = DomainData.LoggedInUsersEntityCollection.FirstOrDefault(r => r.Guid.Equals(registerGuid));
            Assert.AreEqual(null, added);
        }
        #endregion
    }
}