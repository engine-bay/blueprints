namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetBlueprint : IQueryHandler<Guid, BlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.Blueprints
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
                        .Where(x => x.Id == id)
                        .Select(blueprint => new BlueprintDto(blueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        .ConfigureAwait(false);
        }
    }
}