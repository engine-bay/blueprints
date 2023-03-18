namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryWorkbooks : IQueryHandler<PaginationParameters, PaginatedDto<WorkbookDto>>
    {
        private readonly BlueprintsEngineQueryDb db;

        public QueryWorkbooks(BlueprintsEngineQueryDb db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<WorkbookDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit.GetValueOrDefault();
            var skip = limit > 0 ? paginationParameters.Skip.GetValueOrDefault() : 0;

            var total = await this.db.Workbooks.CountAsync(cancellation).ConfigureAwait(false);

            var workbookDtos = limit > 0 ? await this.db.Workbooks
                .Include(x => x.Blueprints)
                        .ThenInclude(blueprint => blueprint.ExpressionBlueprints)
                            .ThenInclude(expressionBlueprint => expressionBlueprint.InputDataTableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.InputDataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.DataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.TriggerExpressionBlueprints)
                            .ThenInclude(x => x.InputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.InputDataVariableBlueprints)
                .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.DataTableColumnBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.DataTableBlueprints)
                        .ThenInclude(x => x.DataTableRowBlueprints)
                            .ThenInclude(x => x.DataTableCellBlueprints)
                .OrderByDescending(x => x.LastUpdatedAt)
                .Skip(skip)
                .Take(limit)
                .Select(workbook => new WorkbookDto(workbook))
                .AsExpandable()
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<WorkbookDto>();

            return new PaginatedDto<WorkbookDto>(total, skip, limit, workbookDtos);
        }
    }
}