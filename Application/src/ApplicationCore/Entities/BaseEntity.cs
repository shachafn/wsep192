using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entitites
{
    public class BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public Guid Guid { get; set; }
        //for concurrency issues
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public BaseEntity()
        {
            Guid = Guid.NewGuid();
        }
    }
}
