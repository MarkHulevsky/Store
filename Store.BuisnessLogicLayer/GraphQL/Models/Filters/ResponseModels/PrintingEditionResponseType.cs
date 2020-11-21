using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Models.PrintingEdition;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;

namespace Store.BuisnessLogic.GraphQL.Models.Filters.ResponseModels
{
    public class PrintingEditionResponseType: ObjectGraphType<PrintingEditionResponseModel>
    {
        public PrintingEditionResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<PrintingEditionType>>(nameof(PrintingEditionResponseModel.PrintingEditions));
        }
    }
}
