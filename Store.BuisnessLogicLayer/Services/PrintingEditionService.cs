using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
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
        private readonly IConfiguration _configuration;
        private readonly IHttpProvider _httpProvider;

        private readonly Mapper<PrintingEditionModel, PrintingEdition> _printingEditionMapper;
        private readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository,
            IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, IConfiguration configuration,
            IHttpProvider httpProvider)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _printingEditionMapper = new Mapper<PrintingEditionModel, PrintingEdition>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
            _configuration = configuration;
            _httpProvider = httpProvider;
        }

        public async Task<decimal> GetConvertRateAsync(string currentCurrency, string newCurrency)
        {
            var baseUrl = _configuration.GetSection("CurrencyOption")["Url"];
            var url = $@"{baseUrl}?base={currentCurrency}&symbols={newCurrency}";
            var jsonResult = JObject.Parse(await _httpProvider.GetHttpContentAsync(url));
            var jTokenRate = jsonResult["rates"][newCurrency];
            var rate = (decimal)jTokenRate;
            return rate;
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
            if (printingEditionModel?.Authors.Count != 0)
            {
                await AddToAuthorsAsync(printingEdition.Id, printingEditionModel.Authors);
            }
            printingEditionModel = _printingEditionModelMapper.Map(printingEdition);
            return printingEditionModel;
        }

        public async Task RemoveAsync(string printingEdiotionId)
        {
            var id = Guid.Parse(printingEdiotionId);
            await _printingEditionRepository.RemoveAsync(id);
        }

        public async Task EditAsync(PrintingEditionModel printingEditionModel)
        {
            var printingEdition = _printingEditionMapper.Map(printingEditionModel);
            await _printingEditionRepository.UpdateAsync(printingEdition);
            if (printingEditionModel.Authors != null)
            {
                await AddToAuthorsAsync(printingEdition.Id, printingEditionModel.Authors);
            }
        }

        public async Task<PrintingEditionResponseModel> FilterAsync(PrintingEditionsRequestModel printingEditionRequestModel)
        {
            var printingEditionRequestDataModel = PrintingEditionRequestMapper.Map(printingEditionRequestModel);
            var printingEditionResponseDataModel = await _printingEditionRepository.FilterAsync(printingEditionRequestDataModel);
            var printingEditionResponseModel = PrintingEditionResponseMapper.Map(printingEditionResponseDataModel);
            return printingEditionResponseModel;
        }

        private async Task AddToAuthorsAsync(Guid printingEditionId, List<AuthorModel> authorModels)
        {
            var authors = ListMapper<Author, AuthorModel>.Map(authorModels);
            var authorInPrintingEditions = new List<AuthorInPrintingEdition>();
            foreach (var author in authors)
            {
                var authorInPrintingEdition = new AuthorInPrintingEdition
                {
                    AuthorId = author.Id,
                    PrintingEditionId = printingEditionId,
                };
                authorInPrintingEditions.Add(authorInPrintingEdition);
            }
            await _authorInPrintingEditionRepository.AddRangeAsync(authorInPrintingEditions);
        }
    }
}
