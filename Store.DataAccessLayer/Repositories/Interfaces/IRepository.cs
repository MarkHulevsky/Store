﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> CreateAsync(T model);
        Task<T> UpdateAsync(T model);
        Task<T> RemoveAsync(Guid id);
    }
}
