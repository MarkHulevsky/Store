using GraphQL.Types;
using Store.BuisnessLogic.Models.PrintingEditions;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class PrintingEditionInputGraphType: InputObjectGraphType<PrintingEditionModel>
    {
        public PrintingEditionInputGraphType()
        {
            Field(x => x.Id, nullable: true);
            Field(x => x.CreationDate, nullable: true);
            Field<ListGraphType<StringGraphType>>(nameof(PrintingEditionModel.Errors));
            Field(x => x.Title, nullable: true);
            Field<FloatGraphType>(nameof(PrintingEditionModel.Price));
            Field(x => x.Description, nullable: true);
            Field<IntGraphType>(nameof(PrintingEditionModel.Type), resolve: context => (int)context.Source.Type);
            Field<IntGraphType>(nameof(PrintingEditionModel.Currency), resolve: context => (int)context.Source.Currency);
            Field<ListGraphType<AuthorInputGraphType>>(nameof(PrintingEditionModel.Authors));
        }
    }
}
