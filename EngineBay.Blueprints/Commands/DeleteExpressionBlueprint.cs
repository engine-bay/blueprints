namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteExpressionBlueprint : ICommandHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsEngineWriteDb db;

        public DeleteExpressionBlueprint(BlueprintsEngineWriteDb db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var expressionBlueprint = await this.db.ExpressionBlueprints
                                    .Include(x => x.InputDataVariableBlueprints)
                                    .Include(x => x.OutputDataVariableBlueprint)
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    .ConfigureAwait(false);

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            this.db.ExpressionBlueprints.Remove(expressionBlueprint);
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}