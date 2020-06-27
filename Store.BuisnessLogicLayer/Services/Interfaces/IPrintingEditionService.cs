using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task CreateAsync(PrintingEditionModel peModel);
        Task RemoveAsync(Guid id);
        Task EditAsync(PrintingEditionModel peModel);
        Task<List<PrintingEditionModel>> FilterAsync(PrintingEditionsRequestFilterModel filter);
        Task<List<PrintingEditionModel>> GetAllAsync();
    }
}
