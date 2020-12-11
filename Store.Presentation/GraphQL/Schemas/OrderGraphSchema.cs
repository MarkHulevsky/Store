using GraphQL.Types;
using GraphQL.Utilities;
using Store.Presentation.GraphQL.Mutations;
using Store.Presentation.GraphQL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Presentation.GraphQL.Schemas
{
    public class OrderGraphSchema: Schema
    {
        public OrderGraphSchema(IServiceProvider serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<OrderGraphQuery>();
            Mutation = serviceProvider.GetRequiredService<OrderGraphMutation>();
        }
    }
}
