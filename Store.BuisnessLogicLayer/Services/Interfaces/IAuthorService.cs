﻿using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorModel>> GetAllAsync();
        Task<AuthorModel> CreateAsync(AuthorModel authorModel);
        Task<AuthorModel> RemoveAsync(string id);
        Task<AuthorModel> EditAsync(AuthorModel authorModel);
        Task<AuthorResponseModel> FilterAsync(AuthorRequestModel authorRequestModel);
    }
}
