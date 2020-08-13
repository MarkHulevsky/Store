using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Base
{
    public abstract class BaseEFRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly ApplicationContext _dbContext;

        public BaseEFRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public virtual async Task<T> CreateAsync(T model)
        {
            model.CreationDate = DateTime.UtcNow;
            var entityEntry = await _dbContext.Set<T>().AddAsync(model);
            model = entityEntry.Entity;
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public virtual async Task<T> RemoveAsync(Guid id)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);
            entity.IsRemoved = true;
            entity = _dbContext.Set<T>().Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id && entity.IsRemoved == false);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().Where(ent => !ent.IsRemoved).ToListAsync();
        }

        public virtual async Task<T> UpdateAsync(T model)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(ent => ent.Id == model.Id);
            entity = _dbContext.Set<T>().Update(model).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

    }
}
