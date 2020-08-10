﻿using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository: IRepository<PrintingEdition>
    {
        PrintingEditionResponseFilter Filter(PrintingEditionsRequestFilter filter);
        Task<List<Author>> GetAuthorsAsync(PrintingEdition printingEdition);
    }
}
