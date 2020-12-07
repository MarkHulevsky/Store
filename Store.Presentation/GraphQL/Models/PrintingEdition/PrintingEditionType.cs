using GraphQL.Types;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.Presentation.GraphQL.Models.Author;
using Store.Presentation.GraphQL.Models.Base;

namespace Store.Presentation.GraphQL.Models.PrintingEdition
{
    public class PrintingEditionType : ObjectGraphType<PrintingEditionModel>
    {
        public PrintingEditionType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(PrintingEditionModel.Errors));
            Field(x => x.Title);
            Field<FloatGraphType>(nameof(PrintingEditionModel.Price));
            Field(x => x.Description);
            Field<IntGraphType>(nameof(PrintingEditionModel.Type), resolve: context => (int)context.Source.Type);
            Field<IntGraphType>(nameof(PrintingEditionModel.Currency), resolve: context => (int)context.Source.Currency);
            Field<ListGraphType<AuthorType>>(nameof(PrintingEditionModel.Authors));
            Interface<BaseInterfaceGraphType>();
            IsTypeOf = obj => obj is PrintingEditionModel;
        }
    }
}
