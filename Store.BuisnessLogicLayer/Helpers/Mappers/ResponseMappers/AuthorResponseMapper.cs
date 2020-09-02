using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFulters;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class AuthorResponseMapper
    {
        private static readonly Mapper<Author, AuthorModel> _authorModelMapper;
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;

        static AuthorResponseMapper()
        {
            _authorModelMapper = new Mapper<Author, AuthorModel>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
        }
        public static AuthorResponseModel Map(AuthorResponseDataModel responseFilter)
        {
            var responseFilterModel = new AuthorResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };
            foreach (var author in responseFilter.Authors)
            {
                var authorModel = _authorModelMapper.Map(author);
                foreach (var pe in author.PrintingEditions)
                {
                    var printingEditionModel = _printingEditionModelMapper.Map(pe);
                    authorModel.PrintingEditions.Add(printingEditionModel);
                }
                responseFilterModel.Authors.Add(authorModel);
            }
            return responseFilterModel;
        }
    }
}
