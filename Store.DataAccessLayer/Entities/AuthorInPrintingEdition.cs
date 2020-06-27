using Store.DataAccessLayer.Entities.Base;
using System;

namespace Store.DataAccessLayer.Entities
{
    public class AuthorInPrintingEdition: BaseEntity
    {
        public Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public Guid PrintingEditionId { get; set; }
        public virtual PrintingEdition PrintingEdition { get; set; }
    }
}
