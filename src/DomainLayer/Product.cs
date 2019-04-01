using DomainLayer;
using System;
using System.Collections.Generic;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    public class Product
    {
        public Guid ProductGuid { get; set; }

        private const int MaxReviewLength = 250;
        private string _name;
        private string _category;
        private double _rate;
        private int _sumOfRates;
        private int _numberOfRates;
        private Dictionary<User, string> _reviews;


        public Product(string name, string category)
        {
            ProductGuid = Guid.NewGuid();
            _name = name;
            _category = category;
            _reviews = new Dictionary<User, string>();
            _rate = 0;
            _sumOfRates = 0;
            _numberOfRates = 0;
        }


        public void AddReview(User user, string review)
        {
            if (CanRateProduct(user) && review.Length <= MaxReviewLength)
                _reviews.Add(user, review);
        }

        public bool RateProduct(User user, int rate)
        {
            if (CanRateProduct(user) && rate >= 1 && rate <= 5)
            {
                _sumOfRates += rate;
                _numberOfRates++;
                rate = _sumOfRates / _numberOfRates;
                return true;
            }
            return false;
        }

        private bool CanRateProduct(User user)
        {
            return user.IsLoggedIn && DomainLayer.Domains.UserDomain.HasPurchasedProduct(this,user.Guid);
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "Name: " + Name + "\nCategory: " + Category + "\nRating: " + Rate;
        }

        public string Name { get; set; }
        public double Rate { get; set; }
        public string Category { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Product)) return false;
            Product p = obj as Product;
            if (p.Name.Equals(this.Name) && p.Category.Equals(this.Category)) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Category);
        }
    }

}