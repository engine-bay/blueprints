namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableBlueprint : ICommandHandler<UpdateParameters<DataTableBlueprint>, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableBlueprint> validator;

        public UpdateDataTableBlueprint(BlueprintsWriteDbContext db, IValidator<DataTableBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(UpdateParameters<DataTableBlueprint> updateParameters, CancellationToken cancellation)
        {
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataTableBlueprint = updateParameters.Entity;

            if (updateDataTableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableBlueprint);

            var dataTableBlueprint = await this.db.DataTableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableBlueprint));
            }

            dataTableBlueprint.Name = updateDataTableBlueprint.Name;
            dataTableBlueprint.Namespace = updateDataTableBlueprint.Namespace;
            dataTableBlueprint.Description = updateDataTableBlueprint.Description;
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}