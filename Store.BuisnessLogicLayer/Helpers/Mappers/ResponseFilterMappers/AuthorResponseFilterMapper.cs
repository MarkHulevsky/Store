using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class AuthorResponseFilterMapper
    {
        private static Mapper<Author, AuthorModel> _authorModelMapper = new Mapper<Author, AuthorModel>();
        private static Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();
        public static AuthorResponseFilterModel Map(AuthorResponseFilter responseFilter)
        {
            var responseFilterModel = new AuthorResponseFilterModel();
            responseFilterModel.TotalCount = responseFilter.TotalCount;
            foreach (var author in responseFilter.Authors)
            {
                var authorModel = _authorModelMapper.Map(new AuthorModel(), author);
                foreach (var pe in author.PrintingEditions)
                {
                    var peModel = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
                    authorModel.PrintingEditions.Add(peModel);
                }
                responseFilterModel.Authors.Add(authorModel);
            }
            return responseFilterModel;
        }
    }
}
