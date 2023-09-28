namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryInputDataVariableBlueprints : PaginatedQuery<InputDataVariableBlueprint>, IQueryHandler<FilteredPaginationParameters<InputDataVariableBlueprint>, PaginatedDto<InputDataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryInputDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<InputDataVariableBlueprintDto>> Handle(FilteredPaginationParameters<InputDataVariableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<InputDataVariableBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.InputDataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.InputDataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<InputDataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.Namespace), StringComparison.OrdinalIgnoreCase) => entity => entity.Namespace,
                string sortBy when sortBy.Equals(nameof(InputDataVariableBlueprint.Type), StringComparison.OrdinalIgnoreCase) => entity => entity.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var inputDataVariableBlueprintDtos = limit > 0 ? await query
                .Select(inputDataVariableBlueprint => new InputDataVariableBlueprintDto(inputDataVariableBlueprint))
                .ToListAsync(cancellation)
                 : new List<InputDataVariableBlueprintDto>();

            return new PaginatedDto<InputDataVariableBlueprintDto>(total, skip, limit, inputDataVariableBlueprintDtos);
        }
    }
}