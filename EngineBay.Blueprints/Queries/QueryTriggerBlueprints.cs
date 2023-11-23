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
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<TriggerBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.TriggerBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.TriggerBlueprints
                            .Include(x => x.TriggerExpressionBlueprints)
                                .ThenInclude(x => x.InputDataVariableBlueprint)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .Where(filterPredicate)
                            .Where(searchPredicate)
                            .AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<TriggerBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(TriggerBlueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var triggerBlueprintDtos = limit > 0 ? await query
                .Select(triggerBlueprint => new TriggerBlueprintDto(triggerBlueprint))
                .ToListAsync(cancellation)
                 : new List<TriggerBlueprintDto>();

            return new PaginatedDto<TriggerBlueprintDto>(total, skip, limit, triggerBlueprintDtos);
        }
    }
}