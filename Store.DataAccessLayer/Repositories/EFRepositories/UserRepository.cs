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

        public async Task<UserResponseDataModel> FilterAsync(UserRequestDataModel filter)
        {
            var users = await DbSet
                .Where(u => !u.IsRemoved && EF.Functions.Like(u.LastName + u.FirstName, $"%{filter.SearchString}%"))
                .Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount)
                .OrderBy($"{filter.SortPropertyName}", $"{filter.SortType}")
                .ToListAsync();
            var result = new UserResponseDataModel
            {
                Users = users,
                TotalCount = DbSet.Where(u => !u.IsRemoved).Count()
            };

            return result;
        }
    }
}
