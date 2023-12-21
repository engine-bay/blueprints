namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableBlueprint : ICommandHandler<DataTableBlueprint, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableBlueprint> validator;

        public CreateDataTableBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(DataTableBlueprint dataTableBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataTableBlueprint);
            await this.db.DataTableBlueprints.AddAsync(dataTableBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}