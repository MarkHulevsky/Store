using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

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
            var sortTypeString = string.Empty;
            if (userRequestDataModel.SortType == SortType.Ascending)
            {
                sortTypeString = Constants.ASCENDING_SORT_TYPE;
            }
            if (userRequestDataModel.SortType == SortType.Descending)
            {
                sortTypeString = Constants.DESCENDING_SORT_TYPE;
            }
            var query = new StringBuilder();
            query.Append($@"SELECT * FROM {tableName} WHERE (FirstName LIKE '%{userRequestDataModel.SearchString}%'
                            OR LastName LIKE '%{userRequestDataModel.SearchString}%') AND IsRemoved = 0 ");
            if (string.IsNullOrWhiteSpace(userRequestDataModel.SortPropertyName))
            {
                userRequestDataModel.SortPropertyName = "Email";
            }
            query.Append($"ORDER BY {userRequestDataModel.SortPropertyName} {sortTypeString} ");
            query.Append($@"OFFSET {userRequestDataModel.Paging.CurrentPage * userRequestDataModel.Paging.ItemsCount} ROWS 
                            FETCH NEXT {userRequestDataModel.Paging.ItemsCount} ROWS ONLY");
            var users = await _dbContext.QueryAsync<User>(query.ToString());

            query.Clear();
            query.Append($"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0");
            var count = await _dbContext.QueryFirstOrDefaultAsync<int>(query.ToString());
            var result = new UserResponseDataModel
            {
                Users = users.ToList(),
                TotalCount = count
            };

            return result;
        }
    }
}
