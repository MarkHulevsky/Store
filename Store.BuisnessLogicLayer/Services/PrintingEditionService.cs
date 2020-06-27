using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogicLayer.Services
{
    public class PrintingEditionService: IPrintingEditionService
    {
        IPrintingEditionRepository _printingEditionRepository;
        IAuthorRepository _authorRepository;
        IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        
        private readonly Mapper<Author, AuthorModel> _authorModelMapper = new Mapper<Author, AuthorModel>();
        private readonly Mapper<AuthorModel, Author> _authorMapper = new Mapper<AuthorModel, Author>();
        private readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        private readonly Mapper<PrintingEditionsRequestFilterModel, PrintingEditionsRequestFilter> _filterMapper =
            new Mapper<PrintingEditionsRequestFilterModel, PrintingEditionsRequestFilter>();
        private readonly Mapper<PrintingEditionModel, PrintingEdition> _printingEditionMapper =
            new Mapper<PrintingEditionModel, PrintingEdition>();
        private readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository,
            IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorRepository= authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
        }

        private async Task<List<AuthorModel>> GetAuthorsAsync(PrintingEdition pe)
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

        public async Task CreateAsync(PrintingEditionModel peModel)
        {
            var pe = _printingEditionMapper.Map(new PrintingEdition(), peModel);
            await _printingEditionRepository.CreateAsync(pe);

            if (peModel.Authors?.Count() != 0)
            {
                foreach (var authorModel in peModel.Authors)
                {
                    await AddToAuthorAsync(peModel, authorModel.Name);
                }
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _printingEditionRepository.RemoveAsync(id);
        }

        public async Task EditAsync(PrintingEditionModel peModel)
        {
            if (peModel.Authors != null)
            {
                foreach(var author in peModel.Authors)
                {
                    await AddToAuthorAsync(peModel, author.Name);
                }
            }
            var pe = _printingEditionMapper.Map(new PrintingEdition(), peModel);
            await _printingEditionRepository.UpdateAsync(pe);
        }

        public async Task<List<PrintingEditionModel>> FilterAsync(PrintingEditionsRequestFilterModel filterModel)
        {
            var paging = _pagingMapper.Map(new Paging(), filterModel.Paging);
            var types = new List<PrintingEditionType>();
            foreach (var type in filterModel.Types)
            {
                types.Add((PrintingEditionType)type);
            }
            var peFilter = _filterMapper.Map(new PrintingEditionsRequestFilter(), filterModel);
            peFilter.Paging = paging;
            peFilter.Types = types;

            var printingEditions = _printingEditionRepository.Filter(peFilter);
            var printingEditionModels = new List<PrintingEditionModel>();
            foreach(var pe in printingEditions)
            {
                var authorsModels = await GetAuthorsAsync(pe);
                var peModel = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
                peModel.Authors = authorsModels;
                printingEditionModels.Add(peModel);
            }

            return printingEditionModels;
        }

        private async Task AddToAuthorAsync(PrintingEditionModel printingEditionModel, string authorName)
        {
            var author = await _authorRepository.FindAuthorByNameAsync(authorName);
            var printingEdition = await _printingEditionRepository
                .GetAsync(printingEditionModel.Id);

            if (author != null & printingEdition != null)
            {
                var authorInPrintingEdition = new AuthorInPrintingEdition
                {
                    Id = Guid.NewGuid(),
                    AuthorId = author.Id,
                    PrintingEditionId = printingEdition.Id
                };
                await _authorInPrintingEditionRepository.CreateAsync(authorInPrintingEdition);
            }
        }

    }
}
