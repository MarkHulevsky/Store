using GraphQL.Types;
using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.GraphQL.Enums
{
    public class OrderStatusGraphType : ObjectGraphType<OrderStatus> { }
    public class PrintingEditionGraphType : ObjectGraphType<PrintingEditionType> { }
    public class CurrencyGraphType : ObjectGraphType<CurrencyType> { }
    public class SortGraphType : ObjectGraphType<SortType> { }
}
