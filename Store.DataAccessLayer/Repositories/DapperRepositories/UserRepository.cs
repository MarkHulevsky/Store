using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class UserRepository : BaseDapperRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.USERS_TABLE_NAME;
        }

        public UserResponseDataModel Filter(UserRequestDataModel filter)
        {
            var query = $"SELECT * FROM {tableName} WHERE (FirstName LIKE '%{filter.SearchString}%'" +
                $"OR LastName LIKE '%{filter.SearchString}%') AND IsRemoved = 0";
            var users = _dbContext.Query<User>(query).ToList();
            var userList = new List<User>().AsQueryable();

            foreach (var status in filter.Statuses)
            {
                userList = userList.Concat(users.Where(u => u.IsActive == status));
            }
            var queryableUsers = userList;
            queryableUsers = queryableUsers.OrderBy($"{filter.SortPropertyName}", $"{filter.SortType}");
            users = queryableUsers.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0";
            var count = _dbContext.QueryFirstOrDefault<int>(query);
            var result = new UserResponseDataModel
            {
                Users = users,
                TotalCount = count
            };

            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var query = $"SELECT * FROM {tableName} WHERE Email = '{email}'";
            var user = await _dbContext.QueryFirstOrDefaultAsync<User>(query, new { email });
            return user;
        }

        public new async Task<User> GetAsync(Guid id)
        {
            var query = $"SELECT * FROM {tableName} WHERE Id = '{id}' AND IsRemoved = 0";
            var user = await _dbContext.QueryFirstOrDefaultAsync<User>(query);
            return user;
        }

        ~UserRepository()
        {
            _dbContext.Close();
        }
    }
}
