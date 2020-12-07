using GraphQL.Types;

namespace Store.Presentation.GraphQL.Queries
{
    public class AppQuery: ObjectGraphType
    {
        public AppQuery()
        {
            Field<AuthorGraphQuery>(nameof(AuthorGraphQuery), resolve: context => new { });
            Field<PrintingEditionGraphQuery>(nameof(PrintingEditionGraphQuery), resolve: context => new { });
        }
    }
}
