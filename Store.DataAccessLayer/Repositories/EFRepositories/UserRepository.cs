using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public UserResponseDataModel Filter(UserRequestDataModel filter)
        {
            var query = DbSet
                .Where(u => !u.IsRemoved && EF.Functions.Like(u.LastName + u.FirstName, $"%{filter.SearchString}%"));

            var subquery = new List<User>().AsQueryable();
            foreach (var status in filter.Statuses)
            {
                subquery = subquery.Concat(query.Where(u => u.IsActive == status));
            }
            query = subquery;

            query = query.OrderBy($"{filter.SortPropertyName}", $"{filter.SortType}");

            var users = query.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();

            var result = new UserResponseDataModel
            {
                Users = users,
                TotalCount = DbSet.Where(u => !u.IsRemoved).Count()
            };

            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
