namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateBlueprint : ICommandHandler<Blueprint, ApplicationUser, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Blueprint> validator;

        public CreateBlueprint(BlueprintsWriteDbContext db, IValidator<Blueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Blueprint blueprint, ApplicationUser user, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(blueprint);
            await this.db.Blueprints.AddAsync(blueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new BlueprintDto(blueprint);
        }
    }
}