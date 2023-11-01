namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableCellBlueprint : ICommandHandler<DataTableCellBlueprint, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableCellBlueprint> validator;

        public CreateDataTableCellBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableCellBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(DataTableCellBlueprint dataTableCellBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataTableCellBlueprint);
            await this.db.DataTableCellBlueprints.AddAsync(dataTableCellBlueprint, cancellation);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}