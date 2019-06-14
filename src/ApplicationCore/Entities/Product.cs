using System.Collections.Generic;

namespace ApplicationCore.Entitites
{
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<string> Keywords { get; set; }

        public Product(string name, string category)
        {
            Name = name;
            Category = category;
        }

        public override string ToString()
        {
            return $"Name - {Name}, Category - {Category} ";
        }
    }
}
