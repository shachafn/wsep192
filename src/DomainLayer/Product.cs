using DomainLayer;
using System;
using System.Collections.Generic;

namespace DomainLayer
{

    public class Product
    {
        private Guid _guid;
        public Guid Guid { get => _guid; set => _guid = value; }

        private const int MaxReviewLength = 250;
        private string _name;
        private string _category;
        private double _rate;
        private int _sumOfRates;
        private int _numberOfRates;
        private Dictionary<User, string> _reviews;


        public Product(string name, string category)
        {
            _guid = Guid.NewGuid();

            this._name = name;
            this._category = category;
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

        public void RateProduct(User user, int rate)
        {
            if (CanRateProduct(user) && rate >= 1 && rate <= 5)
            {
                _sumOfRates += rate;
                _numberOfRates++;
                rate = _sumOfRates / _numberOfRates;
            }
        }

        private bool CanRateProduct(User user)
        {
            return user.IsLogged() && user.HasPurchasedProduct(this);
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