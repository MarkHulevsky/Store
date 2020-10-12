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
            var entityEntry = await DbSet.AddAsync(model);
            model = entityEntry.Entity;
            await SaveChangesAsync();
            return model;
        }

        public virtual async Task<T> RemoveAsync(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(entity => entity.Id == id);
            if (entity == null)
            {
                return entity;
            }
            entity.IsRemoved = true;
            entity = DbSet.Update(entity).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            var result = await DbSet.FirstOrDefaultAsync(entity => entity.Id == id && !entity.IsRemoved);
            return result;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            var result = await DbSet.Where(ent => !ent.IsRemoved).ToListAsync();
            return result;
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(ent => ent.Id == model.Id);
            if (entity == null)
            {
                return entity;
            }
            entity = _dbContext.Set<T>().Update(model).Entity;
            await SaveChangesAsync();
            return entity;
        }

        protected async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
