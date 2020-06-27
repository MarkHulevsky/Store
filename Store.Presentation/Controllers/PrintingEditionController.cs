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

        [HttpGet]
        public async Task<IActionResult> GetFiltred([FromBody] PrintingEditionsRequestFilterModel filter)
        {
            return Ok(await _printingEditionService.FilterAsync(filter));
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task AddPrintingEdition([FromBody] PrintingEditionModel pe)
        {
            await _printingEditionService.CreateAsync(pe);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task DeletePrintingEdition([FromBody] string id)
        {
            await _printingEditionService.RemoveAsync(Guid.Parse(id));
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task EditPrintingEdition([FromBody] PrintingEditionModel pe)
        {
            await _printingEditionService.EditAsync(pe);
        }
    }
}
