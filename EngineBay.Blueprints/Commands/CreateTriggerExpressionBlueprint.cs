namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTriggerExpressionBlueprint : ICommandHandler<TriggerExpressionBlueprint, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateTriggerExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<TriggerExpressionBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(TriggerExpressionBlueprint triggerExpressionBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(triggerExpressionBlueprint);
            await this.db.TriggerExpressionBlueprints.AddAsync(triggerExpressionBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}