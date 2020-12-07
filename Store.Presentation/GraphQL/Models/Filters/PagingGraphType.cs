using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;

namespace Store.Presentation.GraphQL.Models.Filters
{
    public class PagingGraphType : InputObjectGraphType<PagingModel>
    {
        public PagingGraphType()
        {
            Field(x => x.CurrentPage);
            Field(x => x.ItemsCount);
        }
    }
}
