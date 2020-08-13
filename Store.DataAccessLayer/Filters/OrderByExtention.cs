using Store.DataAccess.Entities.Constants;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Store.DataAccessLayer.Filters
{
    static class OrderByExtention
    {
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key, string sortType)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            if (sortType == Constants.descendingSortTypeName)
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
