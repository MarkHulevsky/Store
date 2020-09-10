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
        protected readonly SqlConnection _dbContext;
        protected readonly IConfiguration _configuration;
        private readonly string _connectionString;
        protected string tableName;

        public BaseDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _dbContext = new SqlConnection(_connectionString);
            _dbContext.Open();
        }

        public virtual async Task<T> CreateAsync(T model)
        {
            await _dbContext.InsertAsync<T>(model);
            return model;
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            if (await _dbContext.UpdateAsync(model))
            {
                return model;
            }
            return null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM {tableName} WHERE IsRemoved != 1";
            var result = await _dbContext.QueryAsync<T>(query);
            return result.ToList();
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            var entity = await _dbContext.GetAsync<T>(id);
            return entity;
        }

        public async Task<T> RemoveAsync(Guid id)
        {
            var entity = await _dbContext.GetAsync<T>(id);
            entity.IsRemoved = true;
            await _dbContext.UpdateAsync(entity);
            return entity;
        }

        ~BaseDapperRepository()
        {
            _dbContext.Close();
        }

    }
}
