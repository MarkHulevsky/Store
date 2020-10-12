using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorModel>> GetAll();
        Task<AuthorModel> CreateAsync(AuthorModel authorModel);
        Task RemoveAsync(string id);
        Task EditAsync(AuthorModel authorModel);
        Task<AuthorResponseModel> FilterAsync(AuthorRequestModel authorRequestModel);
    }
}
