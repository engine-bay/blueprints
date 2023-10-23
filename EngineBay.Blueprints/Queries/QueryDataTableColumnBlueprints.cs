namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableColumnBlueprints : PaginatedQuery<DataTableColumnBlueprint>, IQueryHandler<FilteredPaginationParameters<DataTableColumnBlueprint>, PaginatedDto<DataTableColumnBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableColumnBlueprints(BlueprintsQueryDbContext db)
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
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<DataTableColumnBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.DataTableColumnBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.DataTableColumnBlueprints.Where(filterPredicate).Where(searchPredicate).AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<DataTableColumnBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(DataTableColumnBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableColumnBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableColumnBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataTableColumnBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(DataTableColumnBlueprint.Type), StringComparison.OrdinalIgnoreCase) => entity => entity.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA130
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataTableColumnBlueprintDtos = limit > 0 ? await query
                .Select(dataTableColumnBlueprint => new DataTableColumnBlueprintDto(dataTableColumnBlueprint))
                .ToListAsync(cancellation)
                 : new List<DataTableColumnBlueprintDto>();

            return new PaginatedDto<DataTableColumnBlueprintDto>(total, skip, limit, dataTableColumnBlueprintDtos);
        }
    }
}