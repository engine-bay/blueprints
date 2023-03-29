namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataTableRowBlueprint : ICommandHandler<UpdateParameters<DataTableRowBlueprint>, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableRowBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateDataTableRowBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableRowBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(UpdateParameters<DataTableRowBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataTableRowBlueprint = updateParameters.Entity;

            if (updateDataTableRowBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataTableRowBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataTableRowBlueprint);

            var dataTableRowBlueprint = await this.db.DataTableRowBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataTableRowBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableRowBlueprint));
            }

            // this is kind of moot at the moment, since data table rows don't have any other properties at the moment.
            // this will be added to when we eventually get to more well thought out data management features.
            // dataTableRowBlueprint.Name = updateDataTableRowBlueprint.Name;
            // dataTableRowBlueprint.Type = updateDataTableRowBlueprint.Type;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableRowBlueprintDto(dataTableRowBlueprint);
        }
    }
}