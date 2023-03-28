namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredDataVariableBlueprints : PaginatedQuery<DataVariableBlueprint>, IQueryHandler<FilteredPaginationParameters<DataVariableBlueprint>, PaginatedDto<DataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataVariableBlueprintDto>> Handle(FilteredPaginationParameters<DataVariableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
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

            var total = await this.db.DataVariableBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataVariableBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<DataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(DataVariableBlueprint.CreatedAt) => dataVariableBlueprint => dataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataVariableBlueprint.LastUpdatedAt) => dataVariableBlueprint => dataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataVariableBlueprint.Name) => dataVariableBlueprint => dataVariableBlueprint.Name,
                nameof(DataVariableBlueprint.Namespace) => dataVariableBlueprint => dataVariableBlueprint.Namespace,
                nameof(DataVariableBlueprint.Type) => dataVariableBlueprint => dataVariableBlueprint.Type,
                nameof(DataVariableBlueprint.DefaultValue) => dataVariableBlueprint => dataVariableBlueprint.DefaultValue,
                nameof(DataVariableBlueprint.Description) => dataVariableBlueprint => dataVariableBlueprint.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataVariableBlueprintDtos = limit > 0 ? await query
                .Select(dataVariableBlueprint => new DataVariableBlueprintDto(dataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataVariableBlueprintDto>();

            return new PaginatedDto<DataVariableBlueprintDto>(total, skip, limit, dataVariableBlueprintDtos);
        }
    }
}