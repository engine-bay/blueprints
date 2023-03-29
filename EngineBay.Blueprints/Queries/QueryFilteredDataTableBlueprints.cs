namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredDataTableBlueprints : PaginatedQuery<DataTableBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableBlueprint>, PaginatedDto<DataTableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredDataTableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableBlueprintDto>> Handle(FilteredPaginationParameters<DataTableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
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

            var total = await this.db.DataTableBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<DataTableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(DataTableBlueprint.CreatedAt) => dataTableBlueprint => dataTableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableBlueprint.LastUpdatedAt) => dataTableBlueprint => dataTableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableBlueprint.Name) => dataTableBlueprint => dataTableBlueprint.Name,
                nameof(DataTableBlueprint.Namespace) => dataTableBlueprint => dataTableBlueprint.Namespace,
                nameof(DataTableBlueprint.Description) => dataTableBlueprint => dataTableBlueprint.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableBlueprintDtos = limit > 0 ? await query
                .Select(dataTableBlueprint => new DataTableBlueprintDto(dataTableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableBlueprintDto>();

            return new PaginatedDto<DataTableBlueprintDto>(total, skip, limit, dataTableBlueprintDtos);
        }
    }
}