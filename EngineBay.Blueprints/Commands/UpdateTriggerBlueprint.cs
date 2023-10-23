namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateTriggerBlueprint : ICommandHandler<UpdateParameters<TriggerBlueprint>, TriggerBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateTriggerBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<TriggerBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(UpdateParameters<TriggerBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateTriggerBlueprint = updateParameters.Entity;

            if (updateTriggerBlueprint is null)
            {
                throw new ArgumentException(nameof(updateTriggerBlueprint));
            }

            this.validator.ValidateAndThrow(updateTriggerBlueprint);

            var triggerBlueprint = await this.db.TriggerBlueprints.FindAsync(new object[] { id }, cancellation);

            if (triggerBlueprint is null)
            {
                throw new ArgumentException(nameof(triggerBlueprint));
            }

            triggerBlueprint.Name = updateTriggerBlueprint.Name;
            triggerBlueprint.Description = updateTriggerBlueprint.Description;

            await this.db.SaveChangesAsync(user, cancellation);
            return new TriggerBlueprintDto(triggerBlueprint);
        }
    }
}