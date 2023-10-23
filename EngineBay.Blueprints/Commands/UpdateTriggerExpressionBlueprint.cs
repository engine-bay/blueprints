namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateTriggerExpressionBlueprint : ICommandHandler<UpdateParameters<TriggerExpressionBlueprint>, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateTriggerExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<TriggerExpressionBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(UpdateParameters<TriggerExpressionBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateTriggerExpressionBlueprint = updateParameters.Entity;

            if (updateTriggerExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(updateTriggerExpressionBlueprint));
            }

            this.validator.ValidateAndThrow(updateTriggerExpressionBlueprint);

            var triggerExpressionBlueprint = await this.db.TriggerExpressionBlueprints.FindAsync(new object[] { id }, cancellation);

            if (triggerExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(triggerExpressionBlueprint));
            }

            triggerExpressionBlueprint.Expression = updateTriggerExpressionBlueprint.Expression;
            triggerExpressionBlueprint.Objective = updateTriggerExpressionBlueprint.Objective;

            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}