using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Models.PrintingEdition;
using Store.BuisnessLogic.Models.Authors;

namespace Store.BuisnessLogic.GraphQL.Models.Author
{
    public class AuthorType: ObjectGraphType<AuthorModel>
    {
        public AuthorType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field(x => x.Name);
            Field<ListGraphType<StringGraphType>>(nameof(AuthorModel.Errors));
            Field<ListGraphType<PrintingEditionType>>(nameof(AuthorModel.PrintingEditions));
        }
    }
}
