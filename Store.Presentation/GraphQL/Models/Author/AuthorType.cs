using GraphQL.Types;
using Store.BuisnessLogic.Models.Authors;
using Store.Presentation.GraphQL.Models.Base;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Models.Author
{
    public class AuthorType : ObjectGraphType<AuthorModel>
    {
        public AuthorType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(AuthorModel.Errors));
            Field(x => x.Name);
            Field<ListGraphType<PrintingEditionType>>(nameof(AuthorModel.PrintingEditions));
            Interface<BaseInterfaceGraphType>();
            IsTypeOf = obj => obj is AuthorModel;
        }
    }
}
