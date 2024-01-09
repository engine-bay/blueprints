namespace EngineBay.Blueprints
{
    using System;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableBlueprints : PaginatedQuery<DataTableBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableBlueprint>, PaginatedDto<DataTableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableBlueprintDto>> Handle(FilteredPaginationParameters<DataTableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<DataTableBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.DataTableBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.DataTableBlueprints.Where(filterPredicate)
                                                    .Where(searchPredicate)
                                                    .Include(x => x.InputDataVariableBlueprints)
                                                    .Include(x => x.DataTableColumnBlueprints)
                                                    .Include(x => x.DataTableRowBlueprints)
                                                        .ThenInclude(x => x.DataTableCellBlueprints)
                                                    .AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<DataTableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                string sortBy when sortBy.Equals(nameof(DataTableBlueprint.Namespace), StringComparison.OrdinalIgnoreCase) => entity => entity.Namespace,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableBlueprintDtos = limit > 0 ? await query
                .Select(dataTableBlueprint => new DataTableBlueprintDto(dataTableBlueprint))
                .ToListAsync(cancellation)
                 : new List<DataTableBlueprintDto>();

            return new PaginatedDto<DataTableBlueprintDto>(total, skip, limit, dataTableBlueprintDtos);
        }
    }
}