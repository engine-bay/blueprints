namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTriggerExpressionBlueprints : PaginatedQuery<TriggerExpressionBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<TriggerExpressionBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryTriggerExpressionBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<TriggerExpressionBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.TriggerExpressionBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.TriggerExpressionBlueprints
                            .Include(x => x.InputDataVariableBlueprint)
                            .AsExpandable();

            Expression<Func<TriggerExpressionBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(TriggerExpressionBlueprint.CreatedAt) => triggerExpressionBlueprint => triggerExpressionBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerExpressionBlueprint.LastUpdatedAt) => triggerExpressionBlueprint => triggerExpressionBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerExpressionBlueprint.Expression) => triggerExpressionBlueprint => triggerExpressionBlueprint.Expression,
                nameof(TriggerExpressionBlueprint.Objective) => triggerExpressionBlueprint => triggerExpressionBlueprint.Objective,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var triggerExpressionBlueprintDtos = limit > 0 ? await query
                .Select(triggerExpressionBlueprint => new TriggerExpressionBlueprintDto(triggerExpressionBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<TriggerExpressionBlueprintDto>();

            return new PaginatedDto<TriggerExpressionBlueprintDto>(total, skip, limit, triggerExpressionBlueprintDtos);
        }
    }
}