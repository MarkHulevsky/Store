using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly Mapper<Author, AuthorModel> _authorModelMapper = new Mapper<Author, AuthorModel>();
        private readonly Mapper<AuthorModel, Author> _authorMapper = new Mapper<AuthorModel, Author>();

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorModel>> GetAll()
        {
            var authors = await _authorRepository.GetAllAsync();
            var authorModels = new List<AuthorModel>();

            foreach (var author in authors)
            {
                var authorModel = _authorModelMapper.Map(new AuthorModel(), author);
                authorModels.Add(authorModel);
            }

            return authorModels;
        }

        public async Task CreateAsync(AuthorModel authorModel)
        {
            var author = await _authorRepository.FindAuthorByNameAsync(authorModel.Name);
            if (author == null)
            {
                author = _authorMapper.Map(new Author(), authorModel);
                await _authorRepository.CreateAsync(author);
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _authorRepository.RemoveAsync(id);
        }

        public async Task EditAsync(AuthorModel authorModel)
        {
            var author = _authorMapper.Map(new Author(), authorModel);
            await _authorRepository.UpdateAsync(author);
        }

        public async Task<AuthorResponseFilterModel> FilterAsync(AuthorRequestFilterModel filterModel)
        {
            var filter = AuthorRequestFilterMapper.Map(filterModel);
            var authorResponse = await _authorRepository.FilterAsync(filter);
            var authorResponseModel = AuthorResponseFilterMapper.Map(authorResponse);
            return authorResponseModel;
        }
    }
}
