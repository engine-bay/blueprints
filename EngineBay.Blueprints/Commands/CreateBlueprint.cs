namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateBlueprint : ICommandHandler<Blueprint, BlueprintDto>
    {
        private readonly BlueprintsEngineWriteDb db;
        private readonly IValidator<Blueprint> validator;

        public CreateBlueprint(BlueprintsEngineWriteDb db, IValidator<Blueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Blueprint blueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(blueprint);
            await this.db.Blueprints.AddAsync(blueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new BlueprintDto(blueprint);
        }
    }
}