using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition authorInPrintingEditon)
        {
            var entity = await DbSet
                .FirstOrDefaultAsync(authorInPrintingEdition => authorInPrintingEdition.AuthorId == authorInPrintingEditon.AuthorId
                    && authorInPrintingEdition.PrintingEditionId == authorInPrintingEditon.PrintingEditionId);
            if (entity != null)
            {
                return entity;
            }
            await DbSet.AddAsync(authorInPrintingEditon);
            await SaveChangesAsync();
            return authorInPrintingEditon;
        }

        public async Task AddRangeAsync(List<AuthorInPrintingEdition> authorInPrintingEditions)
        {
            await DbSet.AddRangeAsync(authorInPrintingEditions);
            await SaveChangesAsync();
        }
    }
}
