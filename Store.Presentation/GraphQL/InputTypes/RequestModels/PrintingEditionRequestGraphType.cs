using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.Presentation.GraphQL.Models.Filters;
using System.Linq;

namespace Store.Presentation.GraphQL.InputTypes.RequestModels
{
    class PrintingEditionRequestGraphType : InputObjectGraphType<PrintingEditionsRequestModel>
    {
        public PrintingEditionRequestGraphType()
        {
            Field(nameof(PrintingEditionsRequestModel.MaxPrice), expression: x => x.MaxPrice);
            Field(nameof(PrintingEditionsRequestModel.MinPrice), expression: x => x.MinPrice);
            Field(x => x.SearchString);
            Field<PagingGraphType>(nameof(PrintingEditionsRequestModel.Paging));
            Field<IntGraphType>(nameof(PrintingEditionsRequestModel.SortType), resolve: context => (int)context.Source.SortType);
            Field<ListGraphType<IntGraphType>>(nameof(PrintingEditionsRequestModel.Types), resolve: context => context.Source.Types.Cast<int>().ToList());
        }
    }
}
