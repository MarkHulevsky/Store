﻿using Dapper.Contrib.Extensions;
using Store.DataAccess.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.DataAccess.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        [Computed]
        public virtual List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        [Computed]
        [NotMapped]
        public virtual List<PrintingEdition> PrintingEditions { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
            PrintingEditions = new List<PrintingEdition>();
        }
    }
}
