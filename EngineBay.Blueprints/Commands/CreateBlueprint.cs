namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateBlueprint : ICommandHandler<Blueprint, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Blueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<Blueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Blueprint blueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(blueprint);
            await this.db.Blueprints.AddAsync(blueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new BlueprintDto(blueprint);
        }
    }
}