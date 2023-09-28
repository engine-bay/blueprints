namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteDataTableColumnBlueprint : ICommandHandler<Guid, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteDataTableColumnBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var dataTableColumnBlueprint = await this.db.DataTableColumnBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableColumnBlueprint));
            }

            this.db.DataTableColumnBlueprints.Remove(dataTableColumnBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}