using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.EFRepositories
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition model)
        {
            var ent = await _dbContext.AuthorInPrintingEditions
                .FirstOrDefaultAsync(ap => ap.AuthorId == model.AuthorId 
                && ap.PrintingEditionId == ap.PrintingEditionId);
            if (ent == null)
            {
                model.CreationDate = DateTime.UtcNow;
                await _dbContext.AuthorInPrintingEditions.AddAsync(model);
                await _dbContext.SaveChangesAsync();
            }
            return model;
        }
    }
}
