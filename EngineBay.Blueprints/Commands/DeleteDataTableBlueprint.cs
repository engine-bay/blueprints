namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteDataTableBlueprint : ICommandHandler<Guid, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteDataTableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            var dataTableBlueprint = await this.db.DataTableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataTableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableBlueprint));
            }

            this.db.DataTableBlueprints.Remove(dataTableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}