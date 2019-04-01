﻿
using System.Collections.Generic;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    public class Admin
    {
        private User _admin;
        private List<UserReport> _reports;

        public Admin(User admin)
        {
            if (!admin.IsAdmin)
            {
                throw new System.Exception("the user is not an admin");
            }
            _admin = admin;
            _reports = new List<UserReport>();
        }

        public void Report(User reporter, string report)
        {
            if (reporter != null && !report.Equals(string.Empty))
            {
                _reports.Add(new UserReport(reporter, report));
            }
        }

        public bool RemoveUser(string username)//depends on the user component
        {
            if (!Domains.UserDomain.ExistsUser(username))
            {
                return false;
            }
            Domains.UserDomain.RemoveUser(username);
            return true;
        }
        public void ViewShopHistory()//depends on shop implementation
        {

        }

        public List<ShoppingBag> ViewUserHistory(User user)
        {
            return user.GetShoppingHistory();
        }

        public void CloseShopPermanently(Shop shop)
        {
            shop.Adminclose();
            //curently now way how to inform users on the changes
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return _admin.ToString();
        }

        private class UserReport
        {
            public User Reporter { get; set; }
            public string ReportText { get; set; }

            public UserReport(User reporter, string reportText)
            {
                Reporter = reporter;
                ReportText = reportText;
            }

        }
    }

}