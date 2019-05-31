using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entitites;
using ApplicationCore.Entities.Users;

namespace DataAccessLayer
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingBag> ShoppingBags { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShopOwner> Owners { get; set; }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<ShopProduct> ShopProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-KRHRPTB;Initial Catalog=Wsep192_2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        //Connection String
        //Data Source=DESKTOP-3MH7VAJ\SQLEXPRESS;Initial Catalog=WSEP192;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        //TODO: After the installation in whole the app,
        //open the appsettings.json file and add DB connection settings
    }
}
