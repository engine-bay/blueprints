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
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var total = await this.db.DataTableRowBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableRowBlueprints.Include(x => x.DataTableCellBlueprints).Where(filterPredicate).AsExpandable();

            Expression<Func<DataTableRowBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(DataTableRowBlueprint.CreatedAt) => dataTableRowBlueprint => dataTableRowBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableRowBlueprint.LastUpdatedAt) => dataTableRowBlueprint => dataTableRowBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableRowBlueprintDtos = limit > 0 ? await query
                .Select(dataTableRowBlueprint => new DataTableRowBlueprintDto(dataTableRowBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableRowBlueprintDto>();

            return new PaginatedDto<DataTableRowBlueprintDto>(total, skip, limit, dataTableRowBlueprintDtos);
        }
    }
}