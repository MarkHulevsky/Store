using System;

namespace Store.DataAccessLayer.Entities.Base
{
    public class BaseEntity: IBaseEntity
    {
        public Guid Id { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
