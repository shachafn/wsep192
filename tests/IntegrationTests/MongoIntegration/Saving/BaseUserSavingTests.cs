using ApplicationCore.Entities.Users;
using ATBridge;
using DataAccessLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.MongoIntegration.Saving
{
    [TestFixture]
    public class BaseUserSavingTests
    {
        static UnitOfWork _unitOfWork;
        static MongoDbContext _context;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            MongoIntegrationData.PBridge.SetRealBridge(new BridgeImpl());
            _unitOfWork = MongoIntegrationData.GetCurrentUnitOfWork();
            _context = MongoIntegrationData.GetCurrentContext();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
        }

        [TearDown]
        public static void TearDown()
        {
        }

        [SetUp]
        public static void SetUp()
        {
            //MongoIntegrationData.PBridge.ClearSystem();
        }


        [Test]
        public static void TestBaseUser()
        {
            var user = SaveUser();
            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();

            _unitOfWork.BaseUserRepository.Add(user);

            var savedUser = _unitOfWork.BaseUserRepository.FindByIdOrNull(user.Guid);

            VerifyBaseUser(user, savedUser);

            session.AbortTransaction();
        }
        public static BaseUser SaveUser()
        {
            BaseUser shop = new BaseUser("username", "pass", false);

            return shop;
        }

        public static void VerifyBaseUser(BaseUser expected, BaseUser actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Guid, actual.Guid);
            Assert.AreEqual(expected._passHash, actual._passHash);
            Assert.AreEqual(expected.Username, actual.Username);
        }
    }
}
