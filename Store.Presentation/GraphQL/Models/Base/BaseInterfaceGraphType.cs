using GraphQL.Types;
using Store.BuisnessLogic.Models.Base;

namespace Store.Presentation.GraphQL.Models.Base
{
    public class BaseInterfaceGraphType : InterfaceGraphType<BaseModel>
    {
        public BaseInterfaceGraphType()
        {
            Name = "Base";
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(BaseModel.Errors));
        }
    }
}
