namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTriggerBlueprints : PaginatedQuery<TriggerBlueprint>, IQueryHandler<FilteredPaginationParameters<TriggerBlueprint>, PaginatedDto<TriggerBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryTriggerBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<TriggerBlueprintDto>> Handle(FilteredPaginationParameters<TriggerBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<TriggerBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.TriggerBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.TriggerBlueprints
                            .Include(x => x.TriggerExpressionBlueprints)
                                .ThenInclude(x => x.InputDataVariableBlueprint)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .Where(filterPredicate)
                            .Where(searchPredicate)
                            .AsExpandable();

            Expression<Func<TriggerBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var triggerBlueprintDtos = limit > 0 ? await query
                .Select(triggerBlueprint => new TriggerBlueprintDto(triggerBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<TriggerBlueprintDto>();

            return new PaginatedDto<TriggerBlueprintDto>(total, skip, limit, triggerBlueprintDtos);
        }
    }
}