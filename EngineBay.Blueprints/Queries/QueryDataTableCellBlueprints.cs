namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableCellBlueprints : PaginatedQuery<DataTableCellBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableCellBlueprint>, PaginatedDto<DataTableCellBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableCellBlueprints(BlueprintsQueryDbContext db)
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
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<DataTableCellBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.DataTableCellBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableCellBlueprints.Where(filterPredicate).Where(searchPredicate).AsExpandable();

            Expression<Func<DataTableCellBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.Namespace), StringComparison.OrdinalIgnoreCase) => entity => entity.Namespace,
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.Key), StringComparison.OrdinalIgnoreCase) => entity => entity.Key,
                string sortBy when sortBy.Equals(nameof(DataTableCellBlueprint.Value), StringComparison.OrdinalIgnoreCase) => entity => entity.Value,
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