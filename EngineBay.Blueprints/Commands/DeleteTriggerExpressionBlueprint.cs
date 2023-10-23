namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteTriggerExpressionBlueprint : ICommandHandler<Guid, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteTriggerExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
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
            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}