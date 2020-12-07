﻿using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using System.Linq;

namespace Store.Presentation.GraphQL.Models.Filters.RequestModels
{
    class PrintingEditionRequestGraphType : InputObjectGraphType<PrintingEditionsRequestModel>
    {
        public PrintingEditionRequestGraphType()
        {
            Field(nameof(PrintingEditionsRequestModel.MaxPrice), expression: x => x.MaxPrice);
            Field(nameof(PrintingEditionsRequestModel.MinPrice), expression: x => x.MinPrice);
            Field(x => x.SearchString);
            Field<PagingGraphType>(nameof(PrintingEditionsRequestModel.Paging));
            Field<ListGraphType<IntGraphType>>(nameof(PrintingEditionsRequestModel.Types), resolve: context => context.Source.Types.Cast<int>().ToList());
        }
    }
}
