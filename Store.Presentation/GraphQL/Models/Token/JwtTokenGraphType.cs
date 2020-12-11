using GraphQL.Types;
using Store.BuisnessLogic.Models.Token;

namespace Store.Presentation.GraphQL.Models.Token
{
    public class JwtTokenGraphType: ObjectGraphType<JwtTokenModel>
    {
        public JwtTokenGraphType()
        {
            Field(x => x.AccessToken);
            Field(x => x.RefreshToken);
        }
    }
}
