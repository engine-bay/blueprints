namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteDataTableRowBlueprint : ICommandHandler<Guid, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteDataTableRowBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var dataTableRowBlueprint = await this.db.DataTableRowBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableRowBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableRowBlueprint));
            }

            this.db.DataTableRowBlueprints.Remove(dataTableRowBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataTableRowBlueprintDto(dataTableRowBlueprint);
        }
    }
}