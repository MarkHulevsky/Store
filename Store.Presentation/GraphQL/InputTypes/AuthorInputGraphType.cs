using GraphQL.Types;
using Store.BuisnessLogic.Models.Authors;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class AuthorInputGraphType: InputObjectGraphType<AuthorModel>
    {
        public AuthorInputGraphType()
        {
            Field(x => x.Id, nullable: true);
            Field(x => x.CreationDate, nullable: true);
            Field<ListGraphType<StringGraphType>>(nameof(AuthorModel.Errors));
            Field(x => x.Name);
            Field<ListGraphType<PrintingEditionInputGraphType>>(nameof(AuthorModel.PrintingEditions));
        }
    }
}
