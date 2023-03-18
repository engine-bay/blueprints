namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataVariableBlueprint : ICommandHandler<UpdateParameters<DataVariableBlueprint>, DataVariableBlueprintDto>
    {
        private readonly BlueprintsEngineWriteDb db;
        private readonly IValidator<DataVariableBlueprint> validator;

        public UpdateDataVariableBlueprint(BlueprintsEngineWriteDb db, IValidator<DataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(UpdateParameters<DataVariableBlueprint> updateParameters, CancellationToken cancellation)
        {
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataVariableBlueprint = updateParameters.Entity;

            if (updateDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataVariableBlueprint);

            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            dataVariableBlueprint.Name = updateDataVariableBlueprint.Name;
            dataVariableBlueprint.Namespace = updateDataVariableBlueprint.Namespace;
            dataVariableBlueprint.Description = updateDataVariableBlueprint.Description;
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}