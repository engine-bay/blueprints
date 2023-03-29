namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableBlueprint : ICommandHandler<UpdateParameters<DataTableBlueprint>, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateDataTableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(UpdateParameters<DataTableBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
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

            var dataTableBlueprint = await this.db.DataTableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataTableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableBlueprint));
            }

            dataTableBlueprint.Name = updateDataTableBlueprint.Name;
            dataTableBlueprint.Namespace = updateDataTableBlueprint.Namespace;
            dataTableBlueprint.Description = updateDataTableBlueprint.Description;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}