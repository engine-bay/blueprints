namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteTriggerExpressionBlueprint : ICommandHandler<Guid, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteTriggerExpressionBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var triggerExpressionBlueprint = await this.db.TriggerExpressionBlueprints
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    ;

            if (triggerExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(triggerExpressionBlueprint));
            }

            this.db.TriggerExpressionBlueprints.Remove(triggerExpressionBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}