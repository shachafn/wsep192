using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ATBridge;
using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.MongoIntegration
{
    public static class MongoIntegrationData
    {
        public static ProxyBridge PBridge { get; set; } = new ProxyBridge();

        static MongoDbContext CurrentContext { get; set; } = new MongoDbContext(GetDatabaseConfiguration());
        public static MongoDbContext GetCurrentContext()
        {
            return CurrentContext;
        }

        public static UnitOfWork GetCurrentUnitOfWork()
        {
            return new UnitOfWork(GetCurrentContext());
        }

        public static UnitOfWork GetNewUnitOfWork()
        {
            return new UnitOfWork(GetNewContext());
        }

        public static MongoDbContext GetNewContext()
        {
            return new MongoDbContext(GetDatabaseConfiguration());
        }

        public static DatabaseConfiguration GetDatabaseConfiguration()
        {
            return new DatabaseConfiguration("mongodb://localhost:37017", "Wsep");
        }

        public static void ResetDatabase(MongoDbContext mongoDbContext)
        {
            mongoDbContext.GetMongoDatabase()
                .GetCollection<Shop>(nameof(Shop))
                .DeleteOne(FilterDefinition<Shop>.Empty);
            mongoDbContext.GetMongoDatabase()
                .GetCollection<BaseUser>(nameof(BaseUser))
                .DeleteOne(FilterDefinition<BaseUser>.Empty);
            mongoDbContext.GetMongoDatabase().
                GetCollection<ShoppingBag>(nameof(ShoppingBag)).
                DeleteOne(FilterDefinition<ShoppingBag>.Empty);
        }
    }
}
