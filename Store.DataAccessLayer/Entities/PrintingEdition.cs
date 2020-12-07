using Dapper.Contrib.Extensions;
using Store.DataAccess.Entities.Base;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public CurrencyType Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        [Computed]
        public virtual List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        [Computed]
        public virtual List<Author> Authors { get; set; }
        public PrintingEdition()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
            Authors = new List<Author>();
        }
    }
}
