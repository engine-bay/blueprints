namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryWorkbooksMetaData : PaginatedQuery<Workbook>, IQueryHandler<FilteredPaginationParameters<Workbook>, PaginatedDto<WorkbookMetaDataDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryWorkbooksMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<WorkbookMetaDataDto>> Handle(FilteredPaginationParameters<Workbook> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<Workbook, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.Workbooks.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.Workbooks
                .Where(filterPredicate)
                .Where(searchPredicate)
                .AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<Workbook, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(Workbook.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(Workbook.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(Workbook.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(Workbook.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(Workbook.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var workbookDtos = limit > 0 ? await query
                .Select(workbook => new WorkbookMetaDataDto(workbook))
                .ToListAsync(cancellation)
                 : new List<WorkbookMetaDataDto>();

            return new PaginatedDto<WorkbookMetaDataDto>(total, skip, limit, workbookDtos);
        }
    }
}