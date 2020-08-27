using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorModel>> GetAll();
        Task CreateAsync(AuthorModel authorModel);
        Task RemoveAsync(Guid id);
        Task EditAsync(AuthorModel authorModel);
        Task<AuthorResponseModel> FilterAsync(AuthorRequestModel filterModel);
    }
}
