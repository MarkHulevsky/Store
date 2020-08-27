﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly IHttpProvider _http;
        public PrintingEditionController(IPrintingEditionService printingEditionService,
            IConfiguration configuration, IHttpProvider http)
        {
            _printingEditionService = printingEditionService;
            _configuration = configuration;
            _http = http;
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
            var baseUrl = _configuration.GetSection("CurrencyOprion")["Url"];
            var url = $@"{baseUrl}?base={currentCurrency}&symbols={newCurrency}";
            var jsonResult = JObject.Parse(await _http.GetHttpContent(url));
            var rate = jsonResult["rates"][newCurrency];
            var result = (float)rate;
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetFiltred([FromBody] PrintingEditionsRequestModel filter)
        {
            var peResponse = _printingEditionService.Filter(filter);
            return Ok(peResponse);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task Add([FromBody] PrintingEditionModel peModel)
        {
            var pe = await _printingEditionService.CreateAsync(peModel);
            peModel.Id = pe.Id;
            await _printingEditionService.AddToAuthorAsync(peModel, peModel.Authors);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task Delete(string id)
        {
            await _printingEditionService.RemoveAsync(Guid.Parse(id));
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task Edit([FromBody] PrintingEditionModel pe)
        {
            await _printingEditionService.EditAsync(pe);
        }


    }
}
