namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using EngineBay.Persistence;

    public class DeleteDataVariableBlueprint : ICommandHandler<Guid, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            this.db.DataVariableBlueprints.Remove(dataVariableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}