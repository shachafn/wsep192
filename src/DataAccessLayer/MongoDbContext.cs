using ApplicationCore.Interfaces.DataAccessLayer;
using MongoDB.Driver;
using System;

namespace DataAccessLayer
{
    public class MongoDbContext : IContext
    {
        public MongoClient MongoClient { get; set; }
        private string _databaseName;
        private ISession _currentSession;
        public MongoDbContext(DatabaseConfiguration configuration)
        {
            MongoClient = (new MongoClient(configuration.ConnectionString));
            _databaseName = configuration.DatabaseName;
        }

        public IMongoDatabase GetMongoDatabase() => MongoClient.GetDatabase(_databaseName);

        public ISession StartSession()
        {
            _currentSession = new MySession(MongoClient.StartSession());
            return _currentSession;
        }

        public ISession GetCurrentSession()
        {
            return _currentSession;
        }
    }
}
