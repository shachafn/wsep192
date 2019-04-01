using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Data.Entitites
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
