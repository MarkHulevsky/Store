using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.ListMappers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;

        private readonly Mapper<PrintingEditionModel, PrintingEdition> _printingEditionMapper;
        private readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository,
            IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _printingEditionMapper = new Mapper<PrintingEditionModel, PrintingEdition>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
        }

        public async Task<PrintingEditionModel> GetByIdAsync(string id)
        {
            var printingEditionId = Guid.Parse(id);
            var printingEdition = await _printingEditionRepository.GetAsync(printingEditionId);
            var printingEditionModel = _printingEditionModelMapper.Map(printingEdition);
            return printingEditionModel;
        }

        public async Task<PrintingEditionModel> CreateAsync(PrintingEditionModel printingEditionModel)
        {
            var printingEdition = _printingEditionMapper.Map(printingEditionModel);
            printingEdition = await _printingEditionRepository.CreateAsync(printingEdition);
            printingEditionModel = _printingEditionModelMapper.Map(printingEdition);
            return printingEditionModel;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _printingEditionRepository.RemoveAsync(id);
        }

        public async Task EditAsync(PrintingEditionModel printingEditionModel)
        {
            if (printingEditionModel.Authors != null)
            {
                await AddToAuthorAsync(printingEditionModel, printingEditionModel.Authors);
            }
            var printingEdition = _printingEditionMapper.Map(printingEditionModel);
            await _printingEditionRepository.UpdateAsync(printingEdition);
        }

        public async Task<PrintingEditionResponseModel> FilterAsync(PrintingEditionsRequestModel printingEditionRequestModel)
        {
            var printingEditionRequestDataModel = PrintingEditionRequestMapper.Map(printingEditionRequestModel);
            var printingEditionResponseDataModel = await _printingEditionRepository.FilterAsync(printingEditionRequestDataModel);
            var printingEditionResponseModel = PrintingEditionResponseMapper.Map(printingEditionResponseDataModel);
            return printingEditionResponseModel;
        }

        public async Task AddToAuthorAsync(PrintingEditionModel printingEditionModel, List<AuthorModel> authorModels)
        {
            var authors = ListMapper<Author, AuthorModel>.Map(authorModels);
            foreach (var author in authors)
            {
                var authorInPrintingEdition = new AuthorInPrintingEdition
                {
                    AuthorId = author.Id,
                    PrintingEditionId = printingEditionModel.Id,
                };
                await _authorInPrintingEditionRepository.CreateAsync(authorInPrintingEdition);
            }
        }

    }
}
