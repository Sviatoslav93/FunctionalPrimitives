using System.Linq.Expressions;

namespace WebApp.Shared.Extensions;

public static class QueryableExtensions
{
    extension<T>(IQueryable<T> query)
    {
        public IQueryable<T> ApplyPaging(IPaginationRequest request)
        {
            if (request.Page <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Page));
            if (request.PageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.PageSize));

            var skip = (request.Page - 1) * request.PageSize;

            return query
                .Skip(skip)
                .Take(request.PageSize);
        }

        public IQueryable<T> ApplySorting(
            Type type,
            IOrderingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OrderBy))
                return query;

            var parameter = Expression.Parameter(type, "x");

            var property = Expression.PropertyOrField(parameter, request.OrderBy);

            var lambda = Expression.Lambda(property, parameter);

            var methodName = request.OrderDescending
                ? "OrderByDescending"
                : "OrderBy";

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                [type, property.Type],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(result);
        }
    }
}
