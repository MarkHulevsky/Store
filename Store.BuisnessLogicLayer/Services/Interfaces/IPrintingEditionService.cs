using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task<PrintingEdition> CreateAsync(PrintingEditionModel peModel);
        Task RemoveAsync(Guid id);
        Task EditAsync(PrintingEditionModel peModel);
        Task<PrintingEditionResponseFilterModel> FilterAsync(PrintingEditionsRequestFilterModel filter);
        Task<List<PrintingEditionModel>> GetAllAsync();
        Task<List<AuthorModel>> GetAuthorsAsync(PrintingEdition pe);
        Task AddToAuthorAsync(PrintingEditionModel printingEditionModel, IEnumerable<AuthorModel> authorModels);
    }
}
