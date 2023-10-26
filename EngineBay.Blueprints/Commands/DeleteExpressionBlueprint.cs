namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteExpressionBlueprint : ICommandHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteExpressionBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var expressionBlueprint = await this.db.ExpressionBlueprints
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    ;

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            this.db.ExpressionBlueprints.Remove(expressionBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}