using System.Linq.Expressions;

namespace Application.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            return queryable.Where(predicate);
        }

        return queryable;
    }

    //public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByProperty, bool desc)
    //{
    //    var type = typeof(T);
    //    var property = type.GetProperty(orderByProperty);
    //    var parameter = Expression.Parameter(type, "p");
    //    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
    //    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
    //    var resultExpression = Expression.Call(typeof(Queryable), desc ? "OrderByDescending" : "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
    //    return source.Provider.CreateQuery<T>(resultExpression);
    //}
}