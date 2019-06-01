using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entitites;
using ApplicationCore.Entities.Users;
using DataAccessLayer.DAOs;

namespace DataAccessLayer
{
    public class RepositoryContext : DbContext
    {
        //public DbSet<BaseEntity> Entities { get; set; }
        public DbSet<ProductDAO> Products { get; set; }
        public DbSet<ShoppingBagDAO> ShoppingBags { get; set; }
        public DbSet<ShoppingCartDAO> ShoppingCarts { get; set; }
        public DbSet<ShopOwnerDAO> Owners { get; set; }
        public DbSet<BaseUserDAO> Users { get; set; }
        public DbSet<ShopProductDAO> ShopProducts { get; set; }

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
