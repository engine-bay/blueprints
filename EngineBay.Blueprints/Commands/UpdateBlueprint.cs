namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class UpdateBlueprint : ICommandHandler<UpdateParameters<Blueprint>, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Blueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<Blueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(UpdateParameters<Blueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);

            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateBlueprint = updateParameters.Entity;

            if (updateBlueprint is null)
            {
                throw new ArgumentException(nameof(updateBlueprint));
            }

            this.validator.ValidateAndThrow(updateBlueprint);

            var blueprint = await this.db.Blueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (blueprint is null)
            {
                throw new ArgumentException(nameof(blueprint));
            }

            blueprint.Name = updateBlueprint.Name;
            blueprint.Description = updateBlueprint.Description;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new BlueprintDto(blueprint);
        }
    }
}