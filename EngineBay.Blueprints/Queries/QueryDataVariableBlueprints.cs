namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataVariableBlueprints : PaginatedQuery<DataVariableBlueprint>, IQueryHandler<FilteredPaginationParameters<DataVariableBlueprint>, PaginatedDto<DataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataVariableBlueprintDto>> Handle(FilteredPaginationParameters<DataVariableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<DataVariableBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.DataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.DataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).AsExpandable();

#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<DataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.Namespace), StringComparison.OrdinalIgnoreCase) => entity => entity.Namespace,
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.Type), StringComparison.OrdinalIgnoreCase) => entity => entity.Type,
                string sortBy when sortBy.Equals(nameof(DataVariableBlueprint.DefaultValue), StringComparison.OrdinalIgnoreCase) => entity => entity.DefaultValue,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var dataVariableBlueprintDtos = limit > 0 ? await query
                .Select(dataVariableBlueprint => new DataVariableBlueprintDto(dataVariableBlueprint))
                .ToListAsync(cancellation)
                 : new List<DataVariableBlueprintDto>();

            return new PaginatedDto<DataVariableBlueprintDto>(total, skip, limit, dataVariableBlueprintDtos);
        }
    }
}