namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredDataTableColumnBlueprints : PaginatedQuery<DataTableColumnBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableColumnBlueprint>, PaginatedDto<DataTableColumnBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredDataTableColumnBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableColumnBlueprintDto>> Handle(FilteredPaginationParameters<DataTableColumnBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate;

            if (filterPredicate is null)
            {
                throw new ArgumentException(nameof(filterPredicate));
            }

            var total = await this.db.DataTableColumnBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableColumnBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<DataTableColumnBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(DataTableColumnBlueprint.CreatedAt) => dataTableColumnBlueprint => dataTableColumnBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableColumnBlueprint.LastUpdatedAt) => dataTableColumnBlueprint => dataTableColumnBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableColumnBlueprint.Name) => dataTableColumnBlueprint => dataTableColumnBlueprint.Name,
                nameof(DataTableColumnBlueprint.Type) => dataTableColumnBlueprint => dataTableColumnBlueprint.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableColumnBlueprintDtos = limit > 0 ? await query
                .Select(dataTableColumnBlueprint => new DataTableColumnBlueprintDto(dataTableColumnBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableColumnBlueprintDto>();

            return new PaginatedDto<DataTableColumnBlueprintDto>(total, skip, limit, dataTableColumnBlueprintDtos);
        }
    }
}