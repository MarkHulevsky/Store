using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IRepository<PrintingEdition>
    {
        PrintingEditionResponseDataModel Filter(PrintingEditionsRequestDataModel filter);
    }
}
