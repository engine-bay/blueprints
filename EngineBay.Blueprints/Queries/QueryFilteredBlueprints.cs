namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredBlueprints : PaginatedQuery<Blueprint>, IQueryHandler<FilteredPaginationParameters<Blueprint>, PaginatedDto<BlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredBlueprints(BlueprintsQueryDbContext db)
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
            var filterPredicate = filteredPaginationParameters.FilterPredicate;

            if (filterPredicate is null)
            {
                throw new ArgumentException("filterPredicate was null");
            }

            var total = await this.db.Blueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

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
                       .AsExpandable();

            Expression<Func<Blueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(Blueprint.CreatedAt) => blueprint => blueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.LastUpdatedAt) => blueprint => blueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.Name) => blueprint => blueprint.Name,
                nameof(Blueprint.Description) => blueprint => blueprint.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

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