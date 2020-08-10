using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Filters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class UserRepository : BaseDapperRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(IConfiguration configuration, UserManager<User> userManager,
           SignInManager<User> signInManager) : base(configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            TableName = "AspNetUsers";
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public new async Task<IdentityResult> CreateAsync(User user)
        {
            return await _userManager.CreateAsync(user);
        }

        public UserResponseFilter Filter(UserRequestFilter filter)
        {
            var query = $"SELECT * FROM {TableName} WHERE (FirstName LIKE '%{filter.SearchString}%'" +
                $"OR LastName LIKE '%{filter.SearchString}%') AND IsRemoved = 0";
            var users = _dbContext.Query<User>(query).ToList();
            var userList = new List<User>().AsQueryable();

            foreach (var status in filter.Statuses)
            {
                userList = userList.Concat(users.Where(u => u.IsActive == status));
            }
            var queryableUsers = userList;
            queryableUsers = queryableUsers.OrderBy($"{filter.PropName}", $"{filter.SortType}");
            users = queryableUsers.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            query = $"SELECT COUNT(*) FROM {TableName}";
            var count = _dbContext.Execute(query) as int?;
            var result = new UserResponseFilter
            {
                Users = users,
                TotalCount = count.Value
            };

            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var query = $"SELECT * FROM {TableName} WHERE Email = '{email}'";
            var user = await _dbContext.QueryFirstOrDefaultAsync<User>(query, new { email });
            return user;
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

        public async Task<List<string>> GetRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user) as List<string>;
            if (roles.Count > 0)
            {
                return roles;
            }
            return new List<string>();
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

        public async Task<SignInResult> SignInAsync(User user)
        {
            return await _signInManager.PasswordSignInAsync(user, user.Password, false, false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public new async Task<IdentityResult> UpdateAsync(User editedUser)
        {
            var query = $"UPDATE {TableName} SET FirstName = '{editedUser.FirstName}', " +
                $"LastName = '{editedUser.LastName}', IsActive = {Convert.ToInt32(editedUser.IsActive)}," +
                $"Email = '{editedUser.Email}', EmailConfirmed = {Convert.ToInt32(editedUser.EmailConfirmed)} " +
                $"WHERE Id = '{editedUser.Id}'";
            var user = await _dbContext.QueryFirstOrDefaultAsync<User>(query);
            if (user != null)
            {
                return new IdentityResult();
            }
            return null;
        }

        public new async Task<User> GetAsync(Guid id)
        {
            var query = $"SELECT * FROM {TableName} WHERE Id = '{id}' AND IsRemoved = 0";
            var user = await _dbContext.QueryFirstOrDefaultAsync<User>(query);
            return user;
        }

        public new async Task<int> RemoveAsync(Guid id)
        {
            var query = $"UPDATE {TableName} SET IsRemoved = 1 WHERE Id = '{id}'";
            var result = await _dbContext.QueryFirstOrDefaultAsync<User>(query);
            if (result != null)
            {
                return 1;
            }
            return 0;
        }

        public new async Task<List<User>> GetAllAsync()
        {
            var query = $"SELECT * FROM ${TableName}";
            var users = _dbContext.Query<User>(query).ToList();
            return users;
        }

        ~UserRepository()
        {
            _dbContext.Close();
        }
    }
}
