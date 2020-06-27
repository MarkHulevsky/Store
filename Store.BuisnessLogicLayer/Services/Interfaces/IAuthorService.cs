using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorModel>> GetAll();
        Task CreateAsync(AuthorModel authorModel);
        Task RemoveAsync(Guid id);
        Task EditAsync(AuthorModel authorModel);
        Task<List<AuthorModel>> FilterAsync(AuthorRequestFilterModel filterModel);
    }
}
