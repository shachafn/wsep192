using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ApplicationCore.Entitites;
using DataAccessLayer.DAOs.Wrappers;

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
        //public ICollection<StringWrapper> Keywords { get; set; }
        public ProductDAO()
        {

        }
        public ProductDAO(Product product)
        {
            Guid = product.GetGuid();
            Name = product.Name;
            Category = product.Category;
            //Keywords = product.Keywords.Select(s => new StringWrapper() { Id = Guid.NewGuid(), Text = s }).ToList();
        }

        public override string ToString()
        {
            return $"Guid - {Guid}, Name - {Name}, Category - {Category} ";
        }
    }
}
