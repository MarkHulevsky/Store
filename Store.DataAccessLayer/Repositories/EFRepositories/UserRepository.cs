using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public List<User> Filter(UserRequestFilter filter)
        {
            var query = _dbContext.Users
                .Where(u => !u.IsRemoved && EF.Functions.Like(u.LastName + u.FirstName, $"%{filter.SearchFilter}%"));

            var uQuery = new List<User>().AsQueryable();
            foreach (var status in filter.Statuses)
            {
                uQuery = uQuery.Concat(query.Where(u => u.Status == status));
            }
            query = uQuery;
            
            query = query.OrderBy($"{filter.PropName}", $"{filter.SortType}");

            return query.Skip(filter.Paging.Number * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
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

        public new async Task<IdentityResult> UpdateAsync(User user)
        {
            var ent = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GetRoleNameAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
            {
                return roles[0];
            }
            return string.Empty;
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
