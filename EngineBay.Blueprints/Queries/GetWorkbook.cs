namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetWorkbook : IQueryHandler<Guid, WorkbookDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetWorkbook(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.Workbooks
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
                .Where(workbook => workbook.Id == id)
                .Select(workbook => new WorkbookDto(workbook))
                .AsExpandable()
                .FirstAsync(cancellation)
                ;
        }
    }
}