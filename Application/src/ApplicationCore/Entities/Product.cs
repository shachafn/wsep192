using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        [Key]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category is required")]
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
