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
            InitRoles().GetAwaiter().GetResult();
            InitUsers().GetAwaiter().GetResult();
            InitPrintingEditions().GetAwaiter().GetResult();
            InitAuthos().GetAwaiter().GetResult();
        }

        private async Task InitRoles()
        {
            var isCreated = await _roleManager.FindByNameAsync(_adminRoleName) != null ||
                _roleManager.FindByNameAsync(_userRoleName)!= null;
            if (isCreated)
            {
                return;
            }
            await _roleManager.CreateAsync(new IdentityRole<Guid>(_adminRoleName));
            await _roleManager.CreateAsync(new IdentityRole<Guid>(_userRoleName));
        }

        private async Task InitUsers()
        {
            if (await _userManager.FindByNameAsync(_adminEmail) != null)
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
            var result = await _userManager.CreateAsync(admin, _adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, _adminRoleName);
            }
        }

        private async Task InitPrintingEditions()
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

            await _dbContext.AddAsync(book);
            await _dbContext.AddAsync(magazine);
            await _dbContext.AddAsync(newspaper);
            await _dbContext.SaveChangesAsync();
        }

        private async Task InitAuthos()
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

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}
