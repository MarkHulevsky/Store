using Microsoft.AspNetCore.Mvc;
using Store.Presentation.GraphQL.Schemas;

namespace Store.Presentation.Controllers.GraphQLControllers
{
    [Route("api/[controller]/[action]")]
    public class AuthorController : Controller
    {
        private readonly AuthorGraphSchema _schema;
        public AuthorController(AuthorGraphSchema schema)
        {
            _schema = schema;
        }

    }
}
