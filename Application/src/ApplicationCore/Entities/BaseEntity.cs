using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    public class BaseEntity
    {
        public Guid Guid { get; set; }
        public BaseEntity()
        {
            Guid = Guid.NewGuid();
        }
        public Guid GetGuid()
        {
            return Guid;
        }
    }
}
