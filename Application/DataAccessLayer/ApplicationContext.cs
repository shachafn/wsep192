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
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        {

        }
        //public DbSet<BaseEntity> Entities { get; set; }
        public DbSet<ProductDAO> Products { get; set; }
        public DbSet<ShoppingBagDAO> ShoppingBags { get; set; }
        public DbSet<ShoppingCartDAO> ShoppingCarts { get; set; }
        public DbSet<ShopOwnerDAO> Owners { get; set; }
        public DbSet<BaseUserDAO> Users { get; set; }
        public DbSet<ShopProductDAO> ShopProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.Owners);
            modelBuilder.Entity<ShopDAO>().HasMany(shop => shop.Managers);
            modelBuilder.Entity<ShopOwnerDAO>().HasOne(owner => owner.Shop);
            modelBuilder.Entity<ShoppingCartDAO>().HasKey(cart => new { cart.UserGuid, cart.ShopGuid });
            modelBuilder.Entity<RecordsPerCartDAO>().HasKey(rec => new { rec.CartGuid, rec.ProductGuid });

            modelBuilder.Entity<ShoppingBagDAO>().HasKey(bag => bag.UserGuid);
            modelBuilder.Entity<ShoppingBagDAO>().HasMany(bag => bag.ShoppingCarts);
        }
        //Connection String
        //Data Source=DESKTOP-3MH7VAJ\SQLEXPRESS;Initial Catalog=WSEP192;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        //TODO: After the installation in whole the app,
        //open the appsettings.json file and add DB connection settings
    }
}
