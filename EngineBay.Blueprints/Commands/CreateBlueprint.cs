namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateBlueprint : ICommandHandler<Blueprint, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Blueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public CreateBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<Blueprint> validator)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Blueprint blueprint, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(blueprint);
            await this.db.Blueprints.AddAsync(blueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new BlueprintDto(blueprint);
        }
    }
}