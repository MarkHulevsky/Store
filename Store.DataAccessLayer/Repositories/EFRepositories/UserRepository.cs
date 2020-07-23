using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Filters;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.EFRepositories
{
    public class UserRepository : BaseEFRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(ApplicationContext context, UserManager<User> userManager,
            SignInManager<User> signInManager) 
            : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserResponseFilter Filter(UserRequestFilter filter)
        {
            var query = _dbContext.Users
                .Where(u => !u.IsRemoved && EF.Functions.Like(u.LastName + u.FirstName, $"%{filter.SearchString}%"));

            var uQuery = new List<User>().AsQueryable();
            foreach (var status in filter.Statuses)
            {
                uQuery = uQuery.Concat(query.Where(u => u.IsActive == status));
            }
            query = uQuery;
            
            query = query.OrderBy($"{filter.PropName}", $"{filter.SortType}");

            var users = query.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();

            var result = new UserResponseFilter
            {
                Users = users,
                TotalCount = _dbContext.Users.Where(u => !u.IsRemoved).Count()
            };

            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await FindByEmailAsync(email);
            var result = new IdentityResult();
            if (user != null && !string.IsNullOrWhiteSpace(token))
            {
                result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            }
            return result;
        }

        public async Task<string> GetFrogotPasswordTokenAsync(string email)
        {
            var user = await FindByEmailAsync(email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            return string.Empty;
        }

        public new async Task<IdentityResult> UpdateAsync(User editedUser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == editedUser.Id);
            user.FirstName = editedUser.FirstName;
            user.LastName= editedUser.LastName;
            user.Email = editedUser.Email;
            user.Password = editedUser.Password;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<string>> GetRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user) as List<string>;
            if (roles.Count > 0)
            {
                return roles;
            }
            return new List<string>();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public new async Task<IdentityResult> CreateAsync(User user)
        {
            return await _userManager.CreateAsync(user, user.Password);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<SignInResult> SignInAsync(User user)
        {
            return await _signInManager.PasswordSignInAsync(user, user.Password, false, false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
