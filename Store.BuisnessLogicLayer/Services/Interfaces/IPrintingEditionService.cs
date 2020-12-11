using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.PrintingEditions;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task<decimal> GetConvertRateAsync(string currentCurrency, string newCurrency);
        Task<PrintingEditionModel> CreateAsync(PrintingEditionModel printingEditionModel);
        Task<string> RemoveAsync(string id);
        Task<PrintingEditionModel> EditAsync(PrintingEditionModel printigEditionModel);
        Task<PrintingEditionResponseModel> FilterAsync(PrintingEditionsRequestModel printingEditionRequestModel);
        Task<PrintingEditionModel> GetByIdAsync(string id);
    }
}
