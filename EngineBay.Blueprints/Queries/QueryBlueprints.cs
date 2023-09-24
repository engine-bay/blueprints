namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryBlueprints : PaginatedQuery<Blueprint>, IQueryHandler<FilteredPaginationParameters<Blueprint>, PaginatedDto<BlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<BlueprintDto>> Handle(FilteredPaginationParameters<Blueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;
            var search = filteredPaginationParameters.Search;
            Expression<Func<Blueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.Blueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.Blueprints
                       .Include(blueprint => blueprint.ExpressionBlueprints)
                           .ThenInclude(expressionBlueprint => expressionBlueprint.InputDataTableBlueprints)
                       .Include(x => x.ExpressionBlueprints)
                           .ThenInclude(x => x.InputDataVariableBlueprints)
                       .Include(x => x.ExpressionBlueprints)
                           .ThenInclude(x => x.OutputDataVariableBlueprint)
                       .Include(x => x.DataVariableBlueprints)
                       .Include(x => x.TriggerBlueprints)
                           .ThenInclude(x => x.TriggerExpressionBlueprints)
                               .ThenInclude(x => x.InputDataVariableBlueprint)
                       .Include(x => x.TriggerBlueprints)
                           .ThenInclude(x => x.OutputDataVariableBlueprint)
                           .Include(x => x.DataTableBlueprints)
                               .ThenInclude(x => x.InputDataVariableBlueprints)
                           .Include(x => x.DataTableBlueprints)
                               .ThenInclude(x => x.DataTableColumnBlueprints)
                       .Include(x => x.DataTableBlueprints)
                           .ThenInclude(x => x.DataTableRowBlueprints)
                               .ThenInclude(x => x.DataTableCellBlueprints)
                        .Where(filterPredicate)
                        .Where(searchPredicate)
                       .AsExpandable();

#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<Blueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(Blueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(Blueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(Blueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(Blueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(Blueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var blueprintDtos = limit > 0 ? await query
                 .Select(blueprint => new BlueprintDto(blueprint))
                 .ToListAsync(cancellation)
                 .ConfigureAwait(false) : new List<BlueprintDto>();

            return new PaginatedDto<BlueprintDto>(total, skip, limit, blueprintDtos);
        }
    }
}