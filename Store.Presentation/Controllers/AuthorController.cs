using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _authorService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody]AuthorRequestFilterModel filterModel)
        {
            var authorsResponse = await _authorService.FilterAsync(filterModel);
             return Ok(authorsResponse);
        }

        [HttpPost]
        public async Task Add([FromBody]AuthorModel authorModel)
        {
            await _authorService.CreateAsync(authorModel);
        }

        [HttpPut]
        public async Task Edit([FromBody]AuthorModel authorModel)
        {
            await _authorService.EditAsync(authorModel);
        }

        [HttpDelete]
        public async Task Delete(string id)
        {
            await _authorService.RemoveAsync(Guid.Parse(id));
        }


    }
}
