using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.Presentation.Models.AccountModels;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly Mapper<EditProfileViewModel, UserModel> _userModelMapper = 
            new Mapper<EditProfileViewModel, UserModel>();
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userModel = await _userService.GetCurrentAsync(HttpContext.User);
            return Ok(userModel);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult GetFiltred([FromBody] UserRequestFilterModel filter)
        {
            var usersResponse = _userService.Filter(filter);
            return Ok(usersResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileViewModel model)
        {
            var userModel = _userModelMapper.Map(new UserModel(), model);
            var result = await _userService.EditAsync(userModel);
            return Ok(result);
        }

        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task Delete(string userId)
        {
            await _userService.RemoveAsync(Guid.Parse(userId));
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
