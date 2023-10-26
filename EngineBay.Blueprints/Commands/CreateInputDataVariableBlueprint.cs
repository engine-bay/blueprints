namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateInputDataVariableBlueprint : ICommandHandler<InputDataVariableBlueprint, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<InputDataVariableBlueprint> validator;

        public CreateInputDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<InputDataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(InputDataVariableBlueprint inputDataVariableBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(inputDataVariableBlueprint);
            await this.db.InputDataVariableBlueprints.AddAsync(inputDataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}