namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateDataVariableBlueprint : ICommandHandler<DataVariableBlueprint, ApplicationUser, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        public CreateDataVariableBlueprint(BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(DataVariableBlueprint dataVariableBlueprint, ApplicationUser user, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataVariableBlueprint);
            await this.db.DataVariableBlueprints.AddAsync(dataVariableBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}