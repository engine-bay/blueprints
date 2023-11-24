namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableRowBlueprints : PaginatedQuery<DataTableRowBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableRowBlueprint>, PaginatedDto<DataTableRowBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableRowBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableRowBlueprintDto>> Handle(FilteredPaginationParameters<DataTableRowBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var total = await this.db.DataTableRowBlueprints.Where(filterPredicate).CountAsync(cancellation);

            var query = this.db.DataTableRowBlueprints.Include(x => x.DataTableCellBlueprints).Where(filterPredicate).AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<DataTableRowBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(DataTableRowBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableRowBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableRowBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableRowBlueprintDtos = limit > 0 ? await query
                .Select(dataTableRowBlueprint => new DataTableRowBlueprintDto(dataTableRowBlueprint))
                .ToListAsync(cancellation)
                 : new List<DataTableRowBlueprintDto>();

            return new PaginatedDto<DataTableRowBlueprintDto>(total, skip, limit, dataTableRowBlueprintDtos);
        }
    }
}