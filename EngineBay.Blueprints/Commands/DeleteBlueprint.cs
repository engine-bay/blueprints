namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteBlueprint : ICommandHandler<Guid, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var blueprint = await this.db.Blueprints
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
                        .ThenInclude(x => x.DataTableColumnBlueprints)
                    .Include(x => x.DataTableBlueprints)
                        .ThenInclude(x => x.DataTableRowBlueprints)
                            .ThenInclude(x => x.DataTableCellBlueprints)
                .Where(blueprint => blueprint.Id == id)
                .AsExpandable()
                .FirstAsync(cancellation)
                ;

            if (blueprint is null)
            {
                throw new ArgumentException(nameof(blueprint));
            }

            this.db.Blueprints.Remove(blueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new BlueprintDto(blueprint);
        }
    }
}