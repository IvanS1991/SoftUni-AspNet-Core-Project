namespace PropertyAds.WebApp.Infrastructure
{
    using System.Linq;

    public static class QueryableExtensions
    {
        public static IQueryable<T> TryApplyPagination<T>(
            this IQueryable<T> queryable,
            int itemsPerPage,
            int page = 0)
        {

            if (page > 1)
            {
                var offset = (page - 1) * itemsPerPage;

                queryable = queryable.Skip(offset);
            }

            queryable = queryable.Take(itemsPerPage);

            return queryable;
        }
    }
}
