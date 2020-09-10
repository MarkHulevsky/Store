using Dapper.Contrib.Extensions;
using System;

namespace Store.DataAccess.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        private Guid _id;
        [Key]
        [Computed]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public bool IsRemoved { get; set; }
        public DateTime CreationDate { get; set; }
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
    }
}
