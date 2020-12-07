using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class PrintingEditionResponseType : ObjectGraphType<PrintingEditionResponseModel>
    {
        public PrintingEditionResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<PrintingEditionType>>(nameof(PrintingEditionResponseModel.PrintingEditions));
        }
    }
}
