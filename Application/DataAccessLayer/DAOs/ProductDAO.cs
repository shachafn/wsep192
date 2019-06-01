using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entitites;
namespace DataAccessLayer.DAOs
{
    [Table("Products")]
    public class ProductDAO 
    {
        [Key, Column(Order = 0)]
        public Guid Guid { get; set; }
        //[Key,Column(Order =1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ICollection<string> Keywords { get; set; }

        public ProductDAO(Product product)
        {
            Guid = product.GetGuid();
            Name = product.Name;
            Category = product.Category;
            Keywords = product.Keywords;
        }

        public override string ToString()
        {
            return $"Guid - {Guid}, Name - {Name}, Category - {Category} ";
        }
    }
}
