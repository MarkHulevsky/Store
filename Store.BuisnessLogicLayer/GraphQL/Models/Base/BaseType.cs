using GraphQL.Types;
using Store.BuisnessLogic.Models.Base;

namespace Store.BuisnessLogic.GraphQL.Models.Base
{
    public class BaseType: ObjectGraphType<BaseModel>
    {
        public BaseType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(BaseModel.Errors));
        }
    }
}
