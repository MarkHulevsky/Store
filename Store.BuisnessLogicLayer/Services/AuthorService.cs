using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.ListMappers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly Mapper<AuthorModel, Author> _authorMapper;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            _authorMapper = new Mapper<AuthorModel, Author>();
        }

        public async Task<List<AuthorModel>> GetAll()
        {
            var authors = await _authorRepository.GetAllAsync();
            var authorModels = ListMapper<AuthorModel, Author>.Map(authors);
            return authorModels;
        }

        public async Task CreateAsync(AuthorModel authorModel)
        {
            var author = await _authorRepository.FindAuthorByNameAsync(authorModel.Name);
            if (author == null)
            {
                author = _authorMapper.Map(authorModel);
                await _authorRepository.CreateAsync(author);
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _authorRepository.RemoveAsync(id);
        }

        public async Task EditAsync(AuthorModel authorModel)
        {
            var author = await _authorRepository.GetAsync(authorModel.Id);
            if (author == null)
            {
                return;
            }
            author.Name = authorModel.Name;
            await _authorRepository.UpdateAsync(author);
        }

        public async Task<AuthorResponseModel> FilterAsync(AuthorRequestModel filterModel)
        {
            var filter = AuthorRequestMapper.Map(filterModel);
            var authorResponse = await _authorRepository.FilterAsync(filter);
            var authorResponseModel = AuthorResponseMapper.Map(authorResponse);
            return authorResponseModel;
        }
    }
}
