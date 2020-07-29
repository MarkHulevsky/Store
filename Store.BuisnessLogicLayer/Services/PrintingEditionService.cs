using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        IPrintingEditionRepository _printingEditionRepository;
        IAuthorRepository _authorRepository;
        IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;

        private readonly Mapper<Author, AuthorModel> _authorModelMapper = new Mapper<Author, AuthorModel>();
        private readonly Mapper<AuthorModel, Author> _authorMapper = new Mapper<AuthorModel, Author>();
        private readonly Mapper<PrintingEditionModel, PrintingEdition> _printingEditionMapper =
            new Mapper<PrintingEditionModel, PrintingEdition>();
        private readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository,
            IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorRepository = authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
        }

        public async Task<List<AuthorModel>> GetAuthorsAsync(PrintingEdition pe)
        {
            var authors = await _printingEditionRepository.GetAuthorsAsync(pe);
            var authorModels = new List<AuthorModel>();
            foreach (var author in authors)
            {
                var authorModel = _authorModelMapper.Map(new AuthorModel(), author);
                authorModels.Add(authorModel);
            }
            return authorModels;
        }

        public async Task<PrintingEditionModel> GetByIdAsync(string id)
        {
            var pe = await _printingEditionRepository.GetAsync(Guid.Parse(id));
            var peModel = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
            peModel.Authors = await GetAuthorsAsync(pe);
            return peModel;
        }

        public async Task<List<PrintingEditionModel>> GetAllAsync()
        {
            var printingEditions = await _printingEditionRepository.GetAllAsync();
            var printingEditionModels = new List<PrintingEditionModel>();
            foreach (var pe in printingEditions)
            {
                var authorModels = await GetAuthorsAsync(pe);
                var peModel = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
                peModel.Authors = authorModels;
                printingEditionModels.Add(peModel);
            }
            return printingEditionModels;
        }

        public async Task<PrintingEdition> CreateAsync(PrintingEditionModel peModel)
        {
            var pe = _printingEditionMapper.Map(new PrintingEdition(), peModel);
            pe = await _printingEditionRepository.CreateAsync(pe);
            return pe;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _printingEditionRepository.RemoveAsync(id);
        }

        public async Task EditAsync(PrintingEditionModel peModel)
        {
            if (peModel.Authors != null)
            {
                await AddToAuthorAsync(peModel, peModel.Authors);
            }
            var pe = _printingEditionMapper.Map(new PrintingEdition(), peModel);
            await _printingEditionRepository.UpdateAsync(pe);
        }

        public async Task<PrintingEditionResponseFilterModel> FilterAsync(PrintingEditionsRequestFilterModel filterModel)
        {
            var peFilter = PrintingEditionRequestFilterMapper.Map(filterModel);
            var printingEditionsResponse = _printingEditionRepository.Filter(peFilter);
            var printingEditionResponseModel = new PrintingEditionResponseFilterModel();
            printingEditionResponseModel.TotalCount = printingEditionsResponse.TotalCount;
            foreach (var pe in printingEditionsResponse.PrintingEditions)
            {
                var authorsModels = await GetAuthorsAsync(pe);
                var peModel = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
                peModel.Authors = authorsModels;
                printingEditionResponseModel.PrintingEditions.Add(peModel);
            }
            return printingEditionResponseModel;
        }

        public async Task AddToAuthorAsync(PrintingEditionModel printingEditionModel, IEnumerable<AuthorModel> authorModels)
        {
            var authors = new List<Author>();
            foreach (var authorModel in authorModels)
            {
                if (authorModel != null)
                {
                    var author = _authorMapper.Map(new Author(), authorModel);
                    authors.Add(author);
                }
            }

            foreach (var author in authors)
            {
                var aInPe = new AuthorInPrintingEdition
                {
                    AuthorId = author.Id,
                    PrintingEditionId = printingEditionModel.Id,
                    Id = Guid.NewGuid()
                };
                await _authorInPrintingEditionRepository.CreateAsync(aInPe);
            }
        }

    }
}
