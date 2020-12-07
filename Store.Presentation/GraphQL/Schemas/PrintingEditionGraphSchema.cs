using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Store.Presentation.GraphQL.Queries;
using System;

namespace Store.Presentation.GraphQL.Schemas
{
    public class PrintingEditionGraphSchema: Schema
    {
        public PrintingEditionGraphSchema(IServiceProvider serviceProvider): base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<PrintingEditionGraphQuery>();
        }
    }
}
