using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class PaymentRepository : BaseEFRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context) : base(context)
        {
        }

    }
}
