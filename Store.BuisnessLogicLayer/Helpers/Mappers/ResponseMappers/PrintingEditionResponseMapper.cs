using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFulters;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class PrintingEditionResponseMapper
    {
        private static readonly Mapper<Author, AuthorModel> _authorModelMapper;
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;

        static PrintingEditionResponseMapper()
        {
            _authorModelMapper = new Mapper<Author, AuthorModel>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
        }

        public static PrintingEditionResponseModel Map(PrintingEditionResponseDataModel responseFilter)
        {
            var responseFilterModel = new PrintingEditionResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };
            foreach (var printingEdition in responseFilter.PrintingEditions)
            {
                var authorModels = new List<AuthorModel>();
                foreach (var author in printingEdition.Authors)
                {
                    var authorModel = _authorModelMapper.Map(author);
                    authorModels.Add(authorModel);
                }
                var printingEditionModel = _printingEditionModelMapper.Map(printingEdition);
                printingEditionModel.Authors = authorModels;
                responseFilterModel.PrintingEditions.Add(printingEditionModel);
            }
            return responseFilterModel;
        }
    }
}
