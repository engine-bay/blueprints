namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetTriggerExpressionBlueprint : IQueryHandler<Guid, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetTriggerExpressionBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.TriggerExpressionBlueprints
                            .Include(x => x.InputDataVariableBlueprint)
                            .Where(triggerExpressionBlueprint => triggerExpressionBlueprint.Id == id)
                            .Select(triggerExpressionBlueprint => new TriggerExpressionBlueprintDto(triggerExpressionBlueprint))
                            .AsExpandable()
                            .FirstAsync(cancellation)
                            ;
        }
    }
}