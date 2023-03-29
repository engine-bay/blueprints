namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableRowBlueprint : ICommandHandler<DataTableRowBlueprint, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableRowBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateDataTableRowBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableRowBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(DataTableRowBlueprint dataTableRowBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(dataTableRowBlueprint);
            await this.db.DataTableRowBlueprints.AddAsync(dataTableRowBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableRowBlueprintDto(dataTableRowBlueprint);
        }
    }
}