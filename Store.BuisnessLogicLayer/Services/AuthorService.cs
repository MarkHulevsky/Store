using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogicLayer.Models.Authors;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly Mapper<Author, AuthorModel> _authorModelMapper = new Mapper<Author, AuthorModel>();
        private readonly Mapper<AuthorModel, Author> _authorMapper = new Mapper<AuthorModel, Author>();
        private readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        private readonly Mapper<AuthorRequestFilterModel, AuthorRequestFilter> _filterMapper
            = new Mapper<AuthorRequestFilterModel, AuthorRequestFilter>();

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

        public async Task<List<AuthorModel>> FilterAsync(AuthorRequestFilterModel filterModel)
        {
            var paging = _pagingMapper.Map(new Paging(), filterModel.Paging);

            var filter = _filterMapper.Map(new AuthorRequestFilter(), filterModel);

            var authors = await _authorRepository.FilterAsync(filter);
            var authorModels = new List<AuthorModel>();

            foreach (var author in authors)
            {
                authorModels.Add(_authorModelMapper.Map(new AuthorModel(), author));
            }
            return authorModels;
        }
    }
}
