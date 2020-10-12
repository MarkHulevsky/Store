using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Base
{
    public class BaseDapperRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly IConfiguration _configuration;
        protected readonly string connectionString;
        protected string tableName;

        public BaseDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public virtual async Task<T> CreateAsync(T model)
        {
            using (var dbContext = new SqlConnection(connectionString))
            {
                await dbContext.InsertAsync(model);
                return model;
            }
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            if (model == null)
            {
                return model;
            }
            using (var dbContext = new SqlConnection(connectionString))
            {
                await dbContext.UpdateAsync(model);
                return model;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM {tableName} WHERE IsRemoved != 1";
            using (var dbContext = new SqlConnection(connectionString))
            {
                var result = await dbContext.QueryAsync<T>(query);
                return result.ToList();
            }

        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            using (var dbContext = new SqlConnection(connectionString))
            {
                var entity = await dbContext.GetAsync<T>(id);
                return entity;
            }
        }

        public async Task<T> RemoveAsync(Guid id)
        {
            using (var dbContext = new SqlConnection(connectionString))
            {
                var entity = await dbContext.GetAsync<T>(id);
                if (entity == null)
                {
                    return entity;
                }
                entity.IsRemoved = true;
                await dbContext.UpdateAsync(entity);
                return entity;
            }
        }
    }
}
