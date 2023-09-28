namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteOutputDataVariableBlueprint : ICommandHandler<Guid, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteOutputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var outputDataVariableBlueprint = await this.db.OutputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(outputDataVariableBlueprint));
            }

            this.db.OutputDataVariableBlueprints.Remove(outputDataVariableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}