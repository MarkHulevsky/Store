using System;

namespace Store.DataAccess.Entities.Base
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        bool IsRemoved { get; set; }
        DateTime CreationDate { get; set; }
    }
}
