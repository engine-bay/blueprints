namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteDataTableCellBlueprint : ICommandHandler<Guid, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteDataTableCellBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var dataTableCellBlueprint = await this.db.DataTableCellBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableCellBlueprint));
            }

            this.db.DataTableCellBlueprints.Remove(dataTableCellBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}