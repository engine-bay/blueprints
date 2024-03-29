namespace EngineBay.Blueprints
{
    using System;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTriggerExpressionBlueprints : PaginatedQuery<TriggerExpressionBlueprint>, IQueryHandler<FilteredPaginationParameters<TriggerExpressionBlueprint>, PaginatedDto<TriggerExpressionBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryTriggerExpressionBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<TriggerExpressionBlueprintDto>> Handle(FilteredPaginationParameters<TriggerExpressionBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<TriggerExpressionBlueprint, bool>>? searchPredicate = entity => entity.Objective != null && EF.Functions.Like(entity.Objective, $"%{search}%");

            var total = await this.db.TriggerExpressionBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.TriggerExpressionBlueprints
                            .Include(x => x.InputDataVariableBlueprint)
                            .Where(filterPredicate)
                            .Where(searchPredicate)
                            .AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<TriggerExpressionBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(TriggerExpressionBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerExpressionBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerExpressionBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerExpressionBlueprint.Expression), StringComparison.OrdinalIgnoreCase) => entity => entity.Expression,
                string sortBy when sortBy.Equals(nameof(TriggerExpressionBlueprint.Objective), StringComparison.OrdinalIgnoreCase) => entity => entity.Objective,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var triggerExpressionBlueprintDtos = limit > 0 ? await query
                .Select(triggerExpressionBlueprint => new TriggerExpressionBlueprintDto(triggerExpressionBlueprint))
                .ToListAsync(cancellation)
                 : new List<TriggerExpressionBlueprintDto>();

            return new PaginatedDto<TriggerExpressionBlueprintDto>(total, skip, limit, triggerExpressionBlueprintDtos);
        }
    }
}