using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class BaseMongoRepository
    {
        protected MongoDbContext _mongoDbContext;

        public BaseMongoRepository(IContext mongoDbContext)
        {
            _mongoDbContext = (MongoDbContext)mongoDbContext;
        }

        public void Add<T>(T entity, ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            collection.InsertOne(((MySession)session).MongoSession, entity);
        }

        public void Delete<T>(T entity, ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            collection.InsertOne(((MySession)session).MongoSession, entity);
        }

        public ICollection<T> FetchAll<T>(ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            return collection.Find<T>(((MySession)session).MongoSession, t => true).ToList();
        }

        public T FindByIdOrNull<T>(Guid guid, ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            return collection.Find<T>(((MySession)session).MongoSession, t => t.Guid.Equals(guid)).FirstOrDefault();
        }

        public void Update<T>(T entity, ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            collection.ReplaceOne(((MySession)session).MongoSession, dbEntity => dbEntity.Guid.Equals(entity.Guid), entity);
        }

        public IQueryable<T> Query<T>(ISession session) where T : BaseEntity
        {
            var collection = _mongoDbContext.GetMongoDatabase().GetCollection<T>(typeof(T).Name);
            return FetchAll<T>(session).AsQueryable();
        }
    }
}
