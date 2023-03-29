namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableCellBlueprint : ICommandHandler<UpdateParameters<DataTableCellBlueprint>, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableCellBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateDataTableCellBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableCellBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(UpdateParameters<DataTableCellBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataTableCellBlueprint = updateParameters.Entity;

            if (updateDataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableCellBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableCellBlueprint);

            var dataTableCellBlueprint = await this.db.DataTableCellBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableCellBlueprint));
            }

            dataTableCellBlueprint.Name = updateDataTableCellBlueprint.Name;
            dataTableCellBlueprint.Namespace = updateDataTableCellBlueprint.Namespace;
            dataTableCellBlueprint.Key = updateDataTableCellBlueprint.Key;
            dataTableCellBlueprint.Value = updateDataTableCellBlueprint.Value;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}