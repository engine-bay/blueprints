namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataVariableBlueprint : ICommandHandler<DataVariableBlueprint, DataVariableBlueprintDto>
    {
        private readonly BlueprintsEngineWriteDb db;
        private readonly IValidator<DataVariableBlueprint> validator;

        public CreateDataVariableBlueprint(BlueprintsEngineWriteDb db, IValidator<DataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(DataVariableBlueprint dataVariableBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataVariableBlueprint);
            await this.db.DataVariableBlueprints.AddAsync(dataVariableBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}