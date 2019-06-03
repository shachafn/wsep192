using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    //[Table("Products")]
    public class Product : BaseEntity
    {
        //[Key, Column(Order = 0)]
        public Guid thisGuid;
        //[Key,Column(Order =1)]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
       // [Timestamp]
        public byte[] RowVersion { get; set; }
        public ICollection<string> Keywords { get; set; }

        public Product(string name, string category)
        {
            thisGuid = base.GetGuid();
            Name = name;
            Category = category;
        }

        public Product()
        {
        }

        public override string ToString()
        {
            return $"Guid - {Guid}, Name - {Name}, Category - {Category} ";
        }
    }
}
