using Microsoft.AspNetCore.Identity;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Initialization
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
            var isCreated = _roleManager.FindByNameAsync(_adminRoleName).Result != null ||
                _roleManager.FindByNameAsync(_userRoleName).Result != null;
            if (isCreated)
            {
                return;
            }
            _roleManager.CreateAsync(new IdentityRole<Guid>(_adminRoleName)).Wait();
            _roleManager.CreateAsync(new IdentityRole<Guid>(_userRoleName)).Wait();
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
            var result = _userManager.CreateAsync(admin, _adminPassword).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, _adminRoleName).Wait();
            }
        }

        private void InitPrintingEditions()
        {
            if (_dbContext.PrintingEditions.Any())
            {
                return;
            }
            var book = new PrintingEdition
            {
                CreationDate = DateTime.Now,
                Currency = CurrencyType.USD,
                IsRemoved = false,
                Title = "Book Title",
                Description = "Book Description",
                Price = 20,
                Type = PrintingEditionType.Book
            };
            var magazine = new PrintingEdition
            {
                CreationDate = DateTime.Now,
                Currency = CurrencyType.USD,
                IsRemoved = false,
                Title = "Magazine Title",
                Description = "Magazine Description",
                Price = 20,
                Type = PrintingEditionType.Magazine
            };
            var newspaper = new PrintingEdition
            {
                CreationDate = DateTime.Now,
                Currency = CurrencyType.USD,
                IsRemoved = false,
                Title = "Newspaper Title",
                Description = "Newspaper Description",
                Price = 20,
                Type = PrintingEditionType.Newspaper
            };

            _dbContext.Add(book);
            _dbContext.Add(magazine);
            _dbContext.Add(newspaper);
            _dbContext.SaveChanges();
        }

        private void InitAuthos()
        {
            if (_dbContext.Authors.Any())
            {
                return;
            }
            var author = new Author
            {
                Name = "Author",
                CreationDate = DateTime.Now,
                IsRemoved = false,
            };

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }
    }
}
