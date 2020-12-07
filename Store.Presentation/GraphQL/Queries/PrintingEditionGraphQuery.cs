using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes.RequestModels;
using Store.Presentation.GraphQL.Models.Filters.ResponseModels;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Queries
{
    public class PrintingEditionGraphQuery : ObjectGraphType
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionGraphQuery(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;

            FieldAsync<PrintingEditionGraphType>(name: "printingEdition",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: async (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    return await _printingEditionService.GetByIdAsync(id.ToString());
                });


            FieldAsync<PrintingEditionResponseGraphType>("printingEditions",
                arguments: new QueryArguments(new QueryArgument<PrintingEditionRequestGraphType> { Name = "filter" }),
                resolve: async (context) =>
                {
                    var filter = context.GetArgument<PrintingEditionsRequestModel>("filter");
                    var result = await _printingEditionService.FilterAsync(filter);
                    return result;
                });
        }
    }
}
