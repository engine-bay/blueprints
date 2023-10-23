namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTriggerBlueprint : ICommandHandler<TriggerBlueprint, TriggerBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateTriggerBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<TriggerBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(TriggerBlueprint triggerBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(triggerBlueprint);
            await this.db.TriggerBlueprints.AddAsync(triggerBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerBlueprintDto(triggerBlueprint);
        }
    }
}