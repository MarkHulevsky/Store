using System;

namespace Store.DataAccess.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreationDate { get; set; }
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }
}
