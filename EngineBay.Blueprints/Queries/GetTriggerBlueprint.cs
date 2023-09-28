namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetTriggerBlueprint : IQueryHandler<Guid, TriggerBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetTriggerBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.TriggerBlueprints
                            .Include(x => x.TriggerExpressionBlueprints)
                                .ThenInclude(x => x.InputDataVariableBlueprint)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .Where(triggerBlueprint => triggerBlueprint.Id == id)
                            .Select(triggerBlueprint => new TriggerBlueprintDto(triggerBlueprint))
                            .AsExpandable()
                            .FirstAsync(cancellation)
                            ;
        }
    }
}