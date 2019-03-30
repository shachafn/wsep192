﻿using DomainLayer;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    [TestFixture]
    public class UserTest1
    {
        User loggedInUser;
        User notLoggedInUser;
        Product football;

        [SetUp]
        public void Setup()
        {
            User.users = new Dictionary<string, User>();
            loggedInUser = User.Register("meni", "123456");
            notLoggedInUser = User.Register("beni", "123456");
            string pass = "123456";
            loggedInUser.Login(loggedInUser.username, pass);
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
}