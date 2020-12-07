using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class PrintingEditionResponseGraphType : ObjectGraphType<PrintingEditionResponseModel>
    {
        public PrintingEditionResponseGraphType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<PrintingEditionGraphType>>(nameof(PrintingEditionResponseModel.PrintingEditions));
        }
    }
}
