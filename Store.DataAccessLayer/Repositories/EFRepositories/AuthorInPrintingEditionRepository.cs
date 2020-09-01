using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(ap => ap.AuthorId == model.AuthorId
                && ap.PrintingEditionId == model.PrintingEditionId);
            if (entity == null)
            {
                await DbSet.AddAsync(model);
                await SaveChangesAsync();
            }
            return model;
        }
    }
}
