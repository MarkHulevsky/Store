using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class PrintingEditionResponseFilterMapper
    {
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _peModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();
        public static PrintingEditionResponseFilterModel Map(PrintingEditionResponseFilter responseFilter)
        {
            var responseFilterModel = new PrintingEditionResponseFilterModel();
            responseFilterModel.TotalCount = responseFilter.TotalCount;
            foreach(var pe in responseFilter.PrintingEditions)
            {
                var peModel = _peModelMapper.Map(new PrintingEditionModel(), pe);
                responseFilterModel.PrintingEditions.Add(peModel);
            }
            return responseFilterModel;
        }
    }
}
