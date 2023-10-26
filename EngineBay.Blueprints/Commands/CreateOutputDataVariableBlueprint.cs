namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateOutputDataVariableBlueprint : ICommandHandler<OutputDataVariableBlueprint, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<OutputDataVariableBlueprint> validator;

        public CreateOutputDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<OutputDataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(OutputDataVariableBlueprint outputDataVariableBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(outputDataVariableBlueprint);
            await this.db.OutputDataVariableBlueprints.AddAsync(outputDataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}