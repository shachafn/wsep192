using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ApplicationCore.Entitites
{
    public class BaseEntity
    {
        [BsonId]
        public Guid Guid { get; set; }

        public BaseEntity()
        {
            Guid = Guid.NewGuid();
        }
    }
}
