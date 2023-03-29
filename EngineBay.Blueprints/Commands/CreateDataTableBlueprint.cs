namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableBlueprint : ICommandHandler<DataTableBlueprint, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateDataTableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(DataTableBlueprint dataTableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(dataTableBlueprint);
            await this.db.DataTableBlueprints.AddAsync(dataTableBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}