using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Mutations
{
    public class PrintingEditionGraphMutation: ObjectGraphType
    {
        public PrintingEditionGraphMutation(IPrintingEditionService printingEditionService)
        {
            FieldAsync<PrintingEditionGraphType>("add",
                arguments: new QueryArguments(new QueryArgument<PrintingEditionInputGraphType> { Name = "printingEdition" }),
                resolve: async (context) =>
                {
                    var printingEdition = context.GetArgument<PrintingEditionModel>("printingEdition");
                    return await printingEditionService.CreateAsync(printingEdition);
                });

            FieldAsync<PrintingEditionGraphType>("edit",
                arguments: new QueryArguments(new QueryArgument<PrintingEditionInputGraphType> { Name = "printingEdition" }),
                resolve: async (context) =>
                {
                    var printingEdition = context.GetArgument<PrintingEditionModel>("printingEdition");
                    return await printingEditionService.EditAsync(printingEdition);
                });

            FieldAsync<StringGraphType>("remove",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: async (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    return await printingEditionService.RemoveAsync(id);
                });
        }
    }
}
