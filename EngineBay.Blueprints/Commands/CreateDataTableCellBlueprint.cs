namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableCellBlueprint : ICommandHandler<DataTableCellBlueprint, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableCellBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateDataTableCellBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableCellBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(DataTableCellBlueprint dataTableCellBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(dataTableCellBlueprint);
            await this.db.DataTableCellBlueprints.AddAsync(dataTableCellBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}