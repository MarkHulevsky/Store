using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class UserRepository : BaseEFRepository<User>, IUserRepository
    {

        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<User> GetAsync(Guid id)
        {
            var user = await DbSet.Include(user => user.Order)
                .FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public async Task<UserResponseDataModel> FilterAsync(UserRequestDataModel userRequestDataModel)
        {
            var users = await DbSet
                .Where(u => !u.IsRemoved && EF.Functions.Like(u.LastName + u.FirstName, $"%{userRequestDataModel.SearchString}%"))
                .OrderBy($"{userRequestDataModel.SortPropertyName}", $"{userRequestDataModel.SortType}")
                .Skip(userRequestDataModel.Paging.CurrentPage * userRequestDataModel.Paging.ItemsCount)
                .Take(userRequestDataModel.Paging.ItemsCount)
                .ToListAsync();
            var totalCount = await DbSet.Where(u => !u.IsRemoved).CountAsync();
            var result = new UserResponseDataModel
            {
                Users = users,
                TotalCount = totalCount
            };
            return result;
        }
    }
}
