namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTriggerBlueprints : PaginatedQuery<TriggerBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<TriggerBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryTriggerBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<TriggerBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.TriggerBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.TriggerBlueprints
                            .Include(x => x.TriggerExpressionBlueprints)
                                .ThenInclude(x => x.InputDataVariableBlueprint)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .AsExpandable();

            Expression<Func<TriggerBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(TriggerBlueprint.CreatedAt) => triggerBlueprint => triggerBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerBlueprint.LastUpdatedAt) => triggerBlueprint => triggerBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(TriggerBlueprint.Name) => triggerBlueprint => triggerBlueprint.Name,
                nameof(TriggerBlueprint.Description) => triggerBlueprint => triggerBlueprint.Description,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var triggerBlueprintDtos = limit > 0 ? await query
                .Select(triggerBlueprint => new TriggerBlueprintDto(triggerBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<TriggerBlueprintDto>();

            return new PaginatedDto<TriggerBlueprintDto>(total, skip, limit, triggerBlueprintDtos);
        }
    }
}