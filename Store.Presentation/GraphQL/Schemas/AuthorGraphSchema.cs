using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Store.Presentation.GraphQL.Mutations;
using Store.Presentation.GraphQL.Queries;
using System;

namespace Store.Presentation.GraphQL.Schemas
{
    public class AuthorGraphSchema : Schema
    {
        public AuthorGraphSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<AuthorGraphQuery>();
            Mutation = provider.GetRequiredService<AuthorGraphMutation>();
        }
    }
}
