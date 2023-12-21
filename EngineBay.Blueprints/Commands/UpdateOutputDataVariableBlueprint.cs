namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateOutputDataVariableBlueprint : ICommandHandler<UpdateParameters<OutputDataVariableBlueprint>, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<OutputDataVariableBlueprint> validator;

        public UpdateOutputDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<OutputDataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(UpdateParameters<OutputDataVariableBlueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

            var id = updateParameters.Id;
            var updateOutputDataVariableBlueprint = updateParameters.Entity;

            if (updateOutputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateOutputDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateOutputDataVariableBlueprint);

            var outputDataVariableBlueprint = await this.db.OutputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(outputDataVariableBlueprint));
            }

            outputDataVariableBlueprint.Name = updateOutputDataVariableBlueprint.Name;
            outputDataVariableBlueprint.Namespace = updateOutputDataVariableBlueprint.Namespace;
            outputDataVariableBlueprint.Type = updateOutputDataVariableBlueprint.Type;
            await this.db.SaveChangesAsync(cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}