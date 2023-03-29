namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableColumnBlueprint : ICommandHandler<UpdateParameters<DataTableColumnBlueprint>, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableColumnBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateDataTableColumnBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableColumnBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(UpdateParameters<DataTableColumnBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataTableColumnBlueprint = updateParameters.Entity;

            if (updateDataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableColumnBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableColumnBlueprint);

            var dataTableColumnBlueprint = await this.db.DataTableColumnBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableColumnBlueprint));
            }

            dataTableColumnBlueprint.Name = updateDataTableColumnBlueprint.Name;
            dataTableColumnBlueprint.Type = updateDataTableColumnBlueprint.Type;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}