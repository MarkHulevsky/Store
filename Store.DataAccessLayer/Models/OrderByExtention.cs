using System;
using System.Linq;
using System.Linq.Expressions;

namespace Store.DataAccess.Filters
{
    static class OrderByExtention
    {
        private const string DESCENDING_SORT_TYPE = "Desc";
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key, string sortType)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            if (sortType == DESCENDING_SORT_TYPE)
            {
                return Queryable.OrderByDescending(query, lambda);
            }

            return Queryable.OrderBy(query, lambda);
        }

        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, propertyName);

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }
    }
}
