using Dapper.Contrib.Extensions;
using Store.DataAccess.Entities.Base;
using System;

namespace Store.DataAccess.Entities
{
    public class AuthorInPrintingEdition : BaseEntity
    {
        public Guid AuthorId { get; set; }
        [Computed]
        public virtual Author Author { get; set; }
        public Guid PrintingEditionId { get; set; }
        [Computed]
        public virtual PrintingEdition PrintingEdition { get; set; }
    }
}
