namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableRowBlueprint : ICommandHandler<DataTableRowBlueprint, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableRowBlueprint> validator;

        public CreateDataTableRowBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableRowBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(DataTableRowBlueprint dataTableRowBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataTableRowBlueprint);
            await this.db.DataTableRowBlueprints.AddAsync(dataTableRowBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableRowBlueprintDto(dataTableRowBlueprint);
        }
    }
}