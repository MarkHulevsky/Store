using GraphQL.Types;
using Store.BuisnessLogic.Models.Token;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class JwtTokenInputGraphType: InputObjectGraphType<JwtTokenModel>
    {
        public JwtTokenInputGraphType()
        {
            Field(x => x.AccessToken);
            Field(x => x.RefreshToken);
        }
    }
}
