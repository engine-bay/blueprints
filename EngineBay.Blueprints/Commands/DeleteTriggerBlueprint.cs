namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteTriggerBlueprint : ICommandHandler<Guid, TriggerBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteTriggerBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
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
            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerBlueprintDto(triggerBlueprint);
        }
    }
}