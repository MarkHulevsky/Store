using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using System;
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
            var id = Guid.Parse(userId);
            await _userService.RemoveAsync(id);
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
