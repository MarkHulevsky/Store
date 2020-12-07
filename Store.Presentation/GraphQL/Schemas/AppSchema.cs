using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Store.Presentation.GraphQL.Queries;
using System;

namespace Store.Presentation.GraphQL.Schemas
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<AppQuery>();
        }
    }
}
