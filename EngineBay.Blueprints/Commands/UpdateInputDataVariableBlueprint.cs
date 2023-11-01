namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateInputDataVariableBlueprint : ICommandHandler<UpdateParameters<InputDataVariableBlueprint>, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<InputDataVariableBlueprint> validator;

        public UpdateInputDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<InputDataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(UpdateParameters<InputDataVariableBlueprint> updateParameters, CancellationToken cancellation)
        {
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateInputDataVariableBlueprint = updateParameters.Entity;

            if (updateInputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateInputDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateInputDataVariableBlueprint);

            var inputDataVariableBlueprint = await this.db.InputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(inputDataVariableBlueprint));
            }

            inputDataVariableBlueprint.Name = updateInputDataVariableBlueprint.Name;
            inputDataVariableBlueprint.Namespace = updateInputDataVariableBlueprint.Namespace;
            inputDataVariableBlueprint.Type = updateInputDataVariableBlueprint.Type;
            await this.db.SaveChangesAsync(cancellation);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}