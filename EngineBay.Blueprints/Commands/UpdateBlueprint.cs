namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateBlueprint : ICommandHandler<UpdateParameters<Blueprint>, BlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Blueprint> validator;

        public UpdateBlueprint(BlueprintsWriteDbContext db, IValidator<Blueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(UpdateParameters<Blueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

            var id = updateParameters.Id;
            var updateBlueprint = updateParameters.Entity;

            if (updateBlueprint is null)
            {
                throw new ArgumentException(nameof(updateBlueprint));
            }

            this.validator.ValidateAndThrow(updateBlueprint);

            var blueprint = await this.db.Blueprints.FindAsync(new object[] { id }, cancellation);

            if (blueprint is null)
            {
                throw new ArgumentException(nameof(blueprint));
            }

            blueprint.Name = updateBlueprint.Name;
            blueprint.Description = updateBlueprint.Description;
            await this.db.SaveChangesAsync(cancellation);
            return new BlueprintDto(blueprint);
        }
    }
}