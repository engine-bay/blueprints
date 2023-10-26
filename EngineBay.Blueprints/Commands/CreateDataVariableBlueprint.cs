namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataVariableBlueprint : ICommandHandler<DataVariableBlueprint, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        public CreateDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(DataVariableBlueprint dataVariableBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataVariableBlueprint);
            await this.db.DataVariableBlueprints.AddAsync(dataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}