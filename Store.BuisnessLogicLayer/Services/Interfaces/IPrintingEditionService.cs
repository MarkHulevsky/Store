using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.PrintingEditions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task<PrintingEditionModel> CreateAsync(PrintingEditionModel printingEditionModel);
        Task RemoveAsync(Guid id);
        Task EditAsync(PrintingEditionModel printigEditionModel);
        Task<PrintingEditionResponseModel> FilterAsync(PrintingEditionsRequestModel printingEditionRequestModel);
        Task AddToAuthorsAsync(PrintingEditionModel printingEditionModel, List<AuthorModel> authorModels);
        Task<PrintingEditionModel> GetByIdAsync(string id);
    }
}
