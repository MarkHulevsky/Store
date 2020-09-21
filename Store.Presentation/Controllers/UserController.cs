using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userModel = await _userService.GetCurrentAsync();
            return Ok(userModel);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] UserRequestModel userRequestModel)
        {
            var usersResponse = await _userService.FilterAsync(userRequestModel);
            return Ok(usersResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileModel editProfileModel)
        {
            var result = await _userService.EditAsync(editProfileModel);
            return Ok(result);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task Delete(string userId)
        {
            await _userService.RemoveAsync(userId);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] UserModel userModel)
        {
            await _userService.ChangeStatusAsync(userModel);
            return Ok(userModel);
        }
    }
}
