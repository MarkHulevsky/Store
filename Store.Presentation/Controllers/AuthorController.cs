using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
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
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] AuthorRequestModel authorRequestModel)
        {
            var authorsResponse = await _authorService.FilterAsync(authorRequestModel);
            return Ok(authorsResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuthorModel authorModel)
        {
            var createdAuthor = await _authorService.CreateAsync(authorModel);
            return Ok(createdAuthor);
        }

        [HttpPut]
        public async Task Edit([FromBody] AuthorModel authorModel)
        {
            await _authorService.EditAsync(authorModel);
        }

        [HttpDelete]
        public async Task Delete(string id)
        {
            await _authorService.RemoveAsync(id);
        }


    }
}
