using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.BuisnessLogic.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        private readonly IConfiguration _configuration;
        private readonly IHttpProvider _httpProvider;
        public PrintingEditionController(IPrintingEditionService printingEditionService,
            IConfiguration configuration, IHttpProvider httpProvider)
        {
            _printingEditionService = printingEditionService;
            _configuration = configuration;
            _httpProvider = httpProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var pe = await _printingEditionService.GetByIdAsync(id);
            return Ok(pe);
        }

        [HttpGet("{currentCurrency}/{newCurrency}")]
        public async Task<IActionResult> ConvertCurrency(string currentCurrency, string newCurrency)
        {
            var baseUrl = _configuration.GetSection("CurrencyOption")["Url"];
            var url = $@"{baseUrl}?base={currentCurrency}&symbols={newCurrency}";
            var jsonResult = JObject.Parse(await _httpProvider.GetHttpContentAsync(url));
            var rate = jsonResult["rates"][newCurrency];
            var result = (decimal)rate;
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] PrintingEditionsRequestModel printingEditionRequestModel)
        {
            var peResponse = await _printingEditionService.FilterAsync(printingEditionRequestModel);
            return Ok(peResponse);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task Add([FromBody] PrintingEditionModel printingEdiotionModel)
        {
            var pe = await _printingEditionService.CreateAsync(printingEdiotionModel);
            printingEdiotionModel.Id = pe.Id;
            await _printingEditionService.AddToAuthorAsync(printingEdiotionModel, printingEdiotionModel.Authors);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task Delete(string id)
        {
            await _printingEditionService.RemoveAsync(Guid.Parse(id));
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task Edit([FromBody] PrintingEditionModel printingEdition)
        {
            await _printingEditionService.EditAsync(printingEdition);
        }


    }
}
