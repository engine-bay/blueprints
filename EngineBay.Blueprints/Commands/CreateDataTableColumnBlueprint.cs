namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableColumnBlueprint : ICommandHandler<DataTableColumnBlueprint, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableColumnBlueprint> validator;

        public CreateDataTableColumnBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableColumnBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(DataTableColumnBlueprint dataTableColumnBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(dataTableColumnBlueprint);
            await this.db.DataTableColumnBlueprints.AddAsync(dataTableColumnBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}