namespace EngineBay.Blueprints
{
    using System.Linq.Expressions;
    using EngineBay.Core;

    public class FilteredPaginationParameters<T> : PaginationParameters
    {
        public FilteredPaginationParameters(int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, Expression<Func<T, bool>>? filterPredicate)
            : base(skip, limit, sortBy, sortOrder)
        {
            this.FilterPredicate = filterPredicate;
        }

        public Expression<Func<T, bool>>? FilterPredicate { get; set; }
    }
}