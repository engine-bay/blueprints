namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;

    public class DeleteInputDataVariableBlueprint : ICommandHandler<Guid, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteInputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            var inputDataVariableBlueprint = await this.db.InputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(inputDataVariableBlueprint));
            }

            this.db.InputDataVariableBlueprints.Remove(inputDataVariableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}