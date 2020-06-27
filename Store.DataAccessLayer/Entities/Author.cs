using Store.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;

namespace Store.DataAccessLayer.Entities
{
    public class Author: BaseEntity
    {
        public string Name { get; set; }
        public virtual List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
