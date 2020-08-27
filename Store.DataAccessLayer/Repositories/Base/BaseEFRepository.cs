using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Base
{
    public abstract class BaseEFRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationContext _dbContext;
        protected DbSet<T> DbSet
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }

        public BaseEFRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public virtual async Task<T> CreateAsync(T model)
        {
            model.CreationDate = DateTime.UtcNow;
            var entityEntry = await DbSet.AddAsync(model);
            model = entityEntry.Entity;
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public virtual async Task<T> RemoveAsync(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(entity => entity.Id == id);
            entity.IsRemoved = true;
            entity = DbSet.Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(entity => entity.Id == id && entity.IsRemoved == false);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await DbSet.Where(ent => !ent.IsRemoved).ToListAsync();
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(ent => ent.Id == model.Id);
            entity = _dbContext.Set<T>().Update(model).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        protected async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
