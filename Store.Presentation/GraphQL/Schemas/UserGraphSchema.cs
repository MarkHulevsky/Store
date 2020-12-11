using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Store.Presentation.GraphQL.Mutations;
using Store.Presentation.GraphQL.Queries;
using System;

namespace Store.Presentation.GraphQL.Schemas
{
    public class UserGraphSchema: Schema
    {
        public UserGraphSchema(IServiceProvider serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<UserGraphQuery>();
            Mutation = serviceProvider.GetRequiredService<UserGraphMutation>();
        }
    }
}
