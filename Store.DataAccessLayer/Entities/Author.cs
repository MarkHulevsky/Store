using Store.DataAccess.Entities.Base;
using System.Collections.Generic;

namespace Store.DataAccess.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public virtual List<PrintingEdition> PrintingEditions { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
            PrintingEditions = new List<PrintingEdition>();
        }
    }
}
