using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Product : BaseEntity
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
            return $"Guid - {Guid}, Name - {Name}, Category - {Category} ";
        }
    }
}
