using System;

namespace ApplicationCore.Entitites
{
    public class BaseEntity
    {
        public Guid Guid { get; set; }

        public BaseEntity()
        {
            Guid = Guid.NewGuid();
        }
    }
}
