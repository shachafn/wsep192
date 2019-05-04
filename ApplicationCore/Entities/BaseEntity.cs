using System;

namespace ApplicationCore.Entities
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
