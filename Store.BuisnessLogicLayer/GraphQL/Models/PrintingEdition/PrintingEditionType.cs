using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Enums;
using Store.BuisnessLogic.GraphQL.Models.Author;
using Store.BuisnessLogic.Models.PrintingEditions;

namespace Store.BuisnessLogic.GraphQL.Models.PrintingEdition
{
    public class PrintingEditionType: ObjectGraphType<PrintingEditionModel>
    {
        public PrintingEditionType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(PrintingEditionModel.Errors));
            Field(x => x.Title);
            Field(x => x.Price);
            Field(x => x.Description);
            Field<PrintingEditionGraphType>(nameof(PrintingEditionModel.Type));
            Field<CurrencyGraphType>(nameof(PrintingEditionModel.Currency));
            Field<ListGraphType<AuthorType>>(nameof(PrintingEditionModel.Authors));
        }
    }
}
