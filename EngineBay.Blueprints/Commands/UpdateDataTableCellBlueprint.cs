namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableCellBlueprint : ICommandHandler<UpdateParameters<DataTableCellBlueprint>, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableCellBlueprint> validator;

        public UpdateDataTableCellBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableCellBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(UpdateParameters<DataTableCellBlueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

            var id = updateParameters.Id;
            var updateDataTableCellBlueprint = updateParameters.Entity;

            if (updateDataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableCellBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableCellBlueprint);

            var dataTableCellBlueprint = await this.db.DataTableCellBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableCellBlueprint));
            }

            dataTableCellBlueprint.Name = updateDataTableCellBlueprint.Name;
            dataTableCellBlueprint.Namespace = updateDataTableCellBlueprint.Namespace;
            dataTableCellBlueprint.Key = updateDataTableCellBlueprint.Key;
            dataTableCellBlueprint.Value = updateDataTableCellBlueprint.Value;
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}