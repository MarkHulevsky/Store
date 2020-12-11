using GraphQL.Types;
using Store.BuisnessLogic.Models.Payments;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class PaymentInputGraphType: InputObjectGraphType<PaymentModel>
    {
        public PaymentInputGraphType()
        {
            Field(x => x.Id, nullable: true);
            Field(x => x.CreationDate, nullable: true);
            Field<ListGraphType<StringGraphType>>(nameof(PaymentModel.Errors));
            Field(x => x.OrderId);
            Field(x => x.TokenId);
            Field(x => x.UserEmail);
            Field(x => x.Amount);
            Field(x => x.CurrencyString);
        }
    }
}
