
using System.Collections.Generic;

namespace DomainLayer
{
    public class Admin
    {
        User admin;
        List<UserReport> reports;
        public Admin(User admin)
        {
            if(!admin.isAdmin())
            {
                throw new System.Exception("the user is not an admin");
            }
            this.admin = admin;
            reports = new List<UserReport>();
        }


       

        public void report(User reporter,string report)
        {
            if(reporter==null || report.Equals(""))
            {
                return;
            }
            reports.Add(new UserReport(reporter, report));
        }

        public bool removeUser(User toRemove)//depends on the user component
        {

        }
        public void viewShopHistory()//depends on shop implementation
        {

        }

        public List<ShoppingBag> viewUserHistory(User user)
        {
            return user.getShoppingHistory();
        }

        public void closeShopPermanently(Shop shop)
        {
            shop.close();
            //curently now way how to inform users on the changes
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return admin.ToString();
        }

        private class UserReport
        {
            public User Reporter { get => Reporter; set => Reporter = value; }
            public string ReportText { get => ReportText; set => ReportText = value; }

            public UserReport(User reporter, string reportText)
            {
                this.Reporter = reporter;
                this.ReportText = reportText;
            }

            
        }
    }

}