using Microsoft.AspNetCore.Identity;
using Store.DataAccess.Initialization;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using System;
using System.Linq;

namespace Store.DataAccessLayer.Initialization
{
    public class DataBaseInitializer : IDataBaseInitializer
    {
        private const string _adminRoleName = "admin";
        private const string _userRoleName = "user";
        private const string _adminEmail = "admin@gmail.com";
        private const string _adminFirstName = "adminName";
        private const string _adminLastName = "adminSurname";
        private const string _adminPassword = "12345_Admin";
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationContext _dbContext;

        public DataBaseInitializer(UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, ApplicationContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public void InitializeDb()
        {
            InitRoles();
            InitUsers();
            InitPrintingEditions();
            InitAuthos();
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

        private void InitPrintingEditions()
        {
            if (_dbContext.PrintingEditions.Count() == 0)
            {
                var book = new PrintingEdition
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Currency = Entities.Enums.Enums.Currency.USD,
                    IsRemoved = false,
                    Title = "Book Title",
                    Description = "Book Description",
                    Price = 20,
                    Type = Entities.Enums.Enums.PrintingEditionType.Book
                };
                var magazine = new PrintingEdition
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Currency = Entities.Enums.Enums.Currency.USD,
                    IsRemoved = false,
                    Title = "Magazine Title",
                    Description = "Magazine Description",
                    Price = 20,
                    Type = Entities.Enums.Enums.PrintingEditionType.Magazine
                };
                var newspaper = new PrintingEdition
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Currency = Entities.Enums.Enums.Currency.USD,
                    IsRemoved = false,
                    Title = "Newspaper Title",
                    Description = "Newspaper Description",
                    Price = 20,
                    Type = Entities.Enums.Enums.PrintingEditionType.Newspaper
                };

                _dbContext.Add(book);
                _dbContext.Add(magazine);
                _dbContext.Add(newspaper);
                _dbContext.SaveChanges();
            }
        }

        private void InitAuthos()
        {
            if (_dbContext.Authors.Count() == 0)
            {
                var author = new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Author",
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                };

                _dbContext.Authors.Add(author);
                _dbContext.SaveChanges();
            }
        }
    }
}
