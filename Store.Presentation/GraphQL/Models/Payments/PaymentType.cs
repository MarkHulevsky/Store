using GraphQL.Types;
using Store.BuisnessLogic.Models.Payments;

namespace Store.Presentation.GraphQL.Models.Payments
{
    public class PaymentType : ObjectGraphType<PaymentModel>
    {
        public PaymentType()
        {
            Field(x => x.Id);
            Field(x => x.Errors);
            Field(x => x.CreationDate);
            Field(x => x.CurrencyString);
            Field(x => x.OrderId);
            Field(x => x.TokenId);
            Field(x => x.UserEmail);
            Field(x => x.Amount);
        }
    }
}
