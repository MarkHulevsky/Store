using Microsoft.AspNetCore.Identity;
using Store.DataAccess.Initialization;
using Store.DataAccessLayer.Entities;
using System;

namespace Store.DataAccessLayer.Initialization
{
	public class DataBaseInitializer: IDataBaseInitializer
	{
		private const string _adminRoleName = "admin";
		private const string _userRoleName = "user";
		private const string _adminEmail = "admin@gmail.com";
		private const string _adminFirstName = "adminName";
		private const string _adminLastName = "adminSurname";
		private const string _adminPassword = "12345_Admin";
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;

		public DataBaseInitializer(UserManager<User> userManager,
			RoleManager<IdentityRole<Guid>> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public void InitializeDb()
		{
			InitRoles();
			InitUsers();
		}

		private void InitRoles()
		{
			if (_roleManager.FindByNameAsync(_adminRoleName).Result == null)
			{
				_roleManager.CreateAsync(new IdentityRole<Guid>(_adminRoleName)).Wait();
			}
			if (_roleManager.FindByNameAsync(_userRoleName).Result == null)
			{
				_roleManager.CreateAsync(new IdentityRole<Guid>(_userRoleName)).Wait();
			}
		}

		private void InitUsers()
		{
			if (_userManager.FindByNameAsync(_adminEmail).Result != null)
			{
				return;
			}
			var admin = new User
			{
				Email = _adminEmail,
				UserName = _adminEmail,
				FirstName = _adminFirstName,
				LastName = _adminLastName,
				EmailConfirmed = true
			};
			var result = _userManager.CreateAsync(admin, _adminPassword);
			if (result.Result.Succeeded)
			{
				_userManager.AddToRoleAsync(admin, _adminRoleName).Wait();
			}
		}
	}
}
