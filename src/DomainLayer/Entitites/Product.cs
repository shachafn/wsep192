using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Data.Entitites
{
    public class Product : BaseEntity, IEquatable<Product>
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<string> Keywords { get; set; }

        public Product(string name, string category)
        {
            Name = name;
            Category = category;
            Keywords = new List<string>();
        }

        public override string ToString()
        {
            return $"Guid - {Guid}, Name - {Name}, Category - {Category} ";
        }

        public bool Equals(Product other)
        {
            if (!Name.Equals(other.Name))
                return false;
            if (!Category.Equals(other.Category))
                return false;
            if (!Enumerable.SequenceEqual(Keywords, other.Keywords))
                return false;

            return true;
        }
    }
}
