namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryBlueprints : PaginatedQuery<Blueprint>, IQueryHandler<PaginationParameters, PaginatedDto<BlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<BlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.Blueprints.CountAsync(cancellation).ConfigureAwait(false);

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
                       .AsExpandable();

            Expression<Func<Blueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(Blueprint.CreatedAt) => blueprint => blueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.LastUpdatedAt) => blueprint => blueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.Name) => blueprint => blueprint.Name,
                nameof(Blueprint.Description) => blueprint => blueprint.Description,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var blueprintDtos = limit > 0 ? await query
                 .Select(blueprint => new BlueprintDto(blueprint))
                 .ToListAsync(cancellation)
                 .ConfigureAwait(false) : new List<BlueprintDto>();

            return new PaginatedDto<BlueprintDto>(total, skip, limit, blueprintDtos);
        }
    }
}