using ApplicationCore.Interfaces.DataAccessLayer;
using MongoDB.Driver;
using System;

namespace DataAccessLayer
{
    public class MySession : ISession
    {
        Guid g = Guid.NewGuid();
        public IClientSessionHandle MongoSession { get; set; }
        public MySession(IClientSessionHandle mongoSession)
        {
            MongoSession = mongoSession;
        }

        public void AbortTransaction()
        {
            MongoSession.AbortTransaction();
        }

        public void CommitTransaction()
        {
            MongoSession.CommitTransaction();
        }

        public bool IsInTransaction()
        {
            return MongoSession.IsInTransaction;
        }

        public void StartTransaction()
        {
            MongoSession.StartTransaction();
        }
    }
}
