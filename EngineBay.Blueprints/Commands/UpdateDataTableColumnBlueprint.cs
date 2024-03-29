namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableColumnBlueprint : ICommandHandler<UpdateParameters<DataTableColumnBlueprint>, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableColumnBlueprint> validator;

        public UpdateDataTableColumnBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableColumnBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(UpdateParameters<DataTableColumnBlueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

            var id = updateParameters.Id;
            var updateDataTableColumnBlueprint = updateParameters.Entity;

            if (updateDataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableColumnBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableColumnBlueprint);

            var dataTableColumnBlueprint = await this.db.DataTableColumnBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableColumnBlueprint));
            }

            dataTableColumnBlueprint.Name = updateDataTableColumnBlueprint.Name;
            dataTableColumnBlueprint.Type = updateDataTableColumnBlueprint.Type;
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}