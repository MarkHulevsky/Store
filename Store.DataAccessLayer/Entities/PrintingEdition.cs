using Store.DataAccessLayer.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public Currency Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        public virtual List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; } = new List<AuthorInPrintingEdition>();
        [NotMapped]
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
