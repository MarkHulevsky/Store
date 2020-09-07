using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IRepository<PrintingEdition>
    {
        Task<PrintingEditionResponseDataModel> FilterAsync(PrintingEditionsRequestDataModel printingEditionRequsetDataModel);
    }
}
