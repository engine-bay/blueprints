namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredDataTableCellBlueprints : PaginatedQuery<DataTableCellBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableCellBlueprint>, PaginatedDto<DataTableCellBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredDataTableCellBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableCellBlueprintDto>> Handle(FilteredPaginationParameters<DataTableCellBlueprint> filteredPaginationParameters, CancellationToken cancellation)
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

            var total = await this.db.DataTableCellBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableCellBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<DataTableCellBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(DataTableCellBlueprint.CreatedAt) => dataTableCellBlueprint => dataTableCellBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableCellBlueprint.LastUpdatedAt) => dataTableCellBlueprint => dataTableCellBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableCellBlueprint.Name) => dataTableCellBlueprint => dataTableCellBlueprint.Name,
                nameof(DataTableCellBlueprint.Namespace) => dataTableCellBlueprint => dataTableCellBlueprint.Namespace,
                nameof(DataTableCellBlueprint.Key) => dataTableCellBlueprint => dataTableCellBlueprint.Key,
                nameof(DataTableCellBlueprint.Value) => dataTableCellBlueprint => dataTableCellBlueprint.Value,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableCellBlueprintDtos = limit > 0 ? await query
                .Select(dataTableCellBlueprint => new DataTableCellBlueprintDto(dataTableCellBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableCellBlueprintDto>();

            return new PaginatedDto<DataTableCellBlueprintDto>(total, skip, limit, dataTableCellBlueprintDtos);
        }
    }
}