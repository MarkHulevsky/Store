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
        private readonly Mapper<Author, AuthorModel> _authorModelMapper;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            _authorMapper = new Mapper<AuthorModel, Author>();
            _authorModelMapper = new Mapper<Author, AuthorModel>();
        }

        public async Task<List<AuthorModel>> GetAll()
        {
            var authors = await _authorRepository.GetAllAsync();
            var authorModels = ListMapper<AuthorModel, Author>.Map(authors);
            return authorModels;
        }

        public async Task<AuthorModel> CreateAsync(AuthorModel authorModel)
        {
            var author = await _authorRepository.FindAuthorByNameAsync(authorModel.Name);
            if (author != null)
            {
                authorModel = _authorModelMapper.Map(author);
                return authorModel;
            }
            author = _authorMapper.Map(authorModel);
            author = await _authorRepository.CreateAsync(author);
            authorModel = _authorModelMapper.Map(author);
            return authorModel;
        }

        public async Task RemoveAsync(string authorId)
        {
            var result = Guid.TryParse(authorId, out var id);
            if (!result)
            {
                return;
            }
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

        public async Task<AuthorResponseModel> FilterAsync(AuthorRequestModel authorRequestModel)
        {
            var authorRequestDataModel = AuthorRequestMapper.Map(authorRequestModel);
            var authorResponseDataModel = await _authorRepository.FilterAsync(authorRequestDataModel);
            var authorResponseModel = AuthorResponseMapper.Map(authorResponseDataModel);
            return authorResponseModel;
        }
    }
}
