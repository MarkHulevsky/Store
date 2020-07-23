using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.BuisnessLogicLayer.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _printingEditionService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] PrintingEditionsRequestFilterModel filter)
        {
            var peResponse = await _printingEditionService.FilterAsync(filter);
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
