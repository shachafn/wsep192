using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entitites;
using ApplicationCore.Entities.Users;
using DataAccessLayer.DAOs;

namespace DataAccessLayer
{
    public class ApplicationContext : DbContext
    {
         public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
         {
         }

        //public DbSet<BaseEntity> Entities { get; set; }
        public DbSet<ProductDAO> Products { get; set; }
        public DbSet<ShoppingBagDAO> ShoppingBags { get; set; }
        public DbSet<ShoppingCartDAO> ShoppingCarts { get; set; }
        public DbSet<ShopOwnerDAO> Owners { get; set; }
        public DbSet<BaseUserDAO> Users { get; set; }
        public DbSet<ShopProductDAO> ShopProducts { get; set; }
        public DbSet<RecordsPerCartDAO> Records { get; set; }

        // To run migrations, uncomment this block and comment the ctor above^^ 
        // Dont forget to revert so the application can run
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connection = @"Data Source=DESKTOP-KRHRPTB;Initial Catalog=Wsepp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //    optionsBuilder.UseSqlServer(connection);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.Owners);
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.Managers);
            modelBuilder.Entity<ShopDAO>().HasIndex(shop => shop.ShopName).IsUnique();
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.ShopProducts);
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.UsersPurchaseHistory);


            // modelBuilder.Entity<ShopOwnerDAO>().HasOne(owner => owner.ShopGuid);
            //modelBuilder.Entity<ShoppingCartDAO>().HasKey(cart => new { cart.UserGuid, cart.ShopGuid });
            modelBuilder.Entity<RecordsPerCartDAO>().HasKey(rec => new { rec.CartGuid, rec.ProductGuid });

            //modelBuilder.Entity<ShoppingBagDAO>().HasKey(bag => new { bag.UserGuid,bag.ShoppingCarts});
            modelBuilder.Entity<ShoppingBagDAO>().HasMany(bag => bag.ShoppingCarts);

            modelBuilder.Entity<ProductDAO>().HasIndex(product => product.Name).IsUnique();
            //modelBuilder.Entity<ProductDAO>().HasMany(product => product.Keywords);

        }
        //Connection String
        //Data Source=DESKTOP-3MH7VAJ\SQLEXPRESS;Initial Catalog=WSEP192;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        //TODO: After the installation in whole the app,
        //open the appsettings.json file and add DB connection settings
    }
}
