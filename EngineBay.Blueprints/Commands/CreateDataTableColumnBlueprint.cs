namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataTableColumnBlueprint : ICommandHandler<DataTableColumnBlueprint, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataTableColumnBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateDataTableColumnBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataTableColumnBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(DataTableColumnBlueprint dataTableColumnBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(dataTableColumnBlueprint);
            await this.db.DataTableColumnBlueprints.AddAsync(dataTableColumnBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}