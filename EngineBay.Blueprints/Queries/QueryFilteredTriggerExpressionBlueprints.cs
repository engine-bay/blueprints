namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredTriggerExpressionBlueprints : PaginatedQuery<TriggerExpressionBlueprint>, IQueryHandler<FilteredPaginationParameters<TriggerExpressionBlueprint>, PaginatedDto<TriggerExpressionBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredTriggerExpressionBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<TriggerExpressionBlueprintDto>> Handle(FilteredPaginationParameters<TriggerExpressionBlueprint> filteredPaginationParameters, CancellationToken cancellation)
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

            var total = await this.db.TriggerExpressionBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.TriggerExpressionBlueprints
                            .Include(x => x.InputDataVariableBlueprint)
                            .Where(filterPredicate)
                            .AsExpandable();

            Expression<Func<TriggerExpressionBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(TriggerExpressionBlueprint.CreatedAt) => triggerExpressionBlueprint => triggerExpressionBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerExpressionBlueprint.LastUpdatedAt) => triggerExpressionBlueprint => triggerExpressionBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerExpressionBlueprint.Expression) => triggerExpressionBlueprint => triggerExpressionBlueprint.Expression,
                nameof(TriggerExpressionBlueprint.Objective) => triggerExpressionBlueprint => triggerExpressionBlueprint.Objective,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var triggerExpressionBlueprintDtos = limit > 0 ? await query
                .Select(triggerExpressionBlueprint => new TriggerExpressionBlueprintDto(triggerExpressionBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<TriggerExpressionBlueprintDto>();

            return new PaginatedDto<TriggerExpressionBlueprintDto>(total, skip, limit, triggerExpressionBlueprintDtos);
        }
    }
}