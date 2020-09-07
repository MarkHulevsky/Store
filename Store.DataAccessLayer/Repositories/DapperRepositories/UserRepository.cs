using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
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

        public async Task<UserResponseDataModel> FilterAsync(UserRequestDataModel userRequestDataModel)
        {
            var query = $"SELECT * FROM {tableName} WHERE (FirstName LIKE '%{userRequestDataModel.SearchString}%'" +
                $"OR LastName LIKE '%{userRequestDataModel.SearchString}%') AND IsRemoved = 0";
            var users = await _dbContext.QueryAsync<User>(query);
            var queryableUsers = users.AsQueryable();

            queryableUsers = queryableUsers
                .OrderBy($"{userRequestDataModel.SortPropertyName}", $"{userRequestDataModel.SortType}")
                .Skip(userRequestDataModel.Paging.CurrentPage * userRequestDataModel.Paging.ItemsCount)
                .Take(userRequestDataModel.Paging.ItemsCount);

            users = queryableUsers.ToList();
            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0";
            var count = await _dbContext.QueryFirstOrDefaultAsync<int>(query);
            var result = new UserResponseDataModel
            {
                Users = users,
                TotalCount = count
            };

            return result;
        }

        public override async Task<User> GetAsync(Guid id)
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
