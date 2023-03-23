namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;

    public class DeleteDataVariableBlueprint : ICommandHandler<Guid, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public DeleteDataVariableBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            this.db.DataVariableBlueprints.Remove(dataVariableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}