namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteTriggerBlueprint : ICommandHandler<Guid, TriggerBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteTriggerBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var triggerBlueprint = await this.db.TriggerBlueprints
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    ;

            if (triggerBlueprint is null)
            {
                throw new ArgumentException(nameof(triggerBlueprint));
            }

            this.db.TriggerBlueprints.Remove(triggerBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new TriggerBlueprintDto(triggerBlueprint);
        }
    }
}