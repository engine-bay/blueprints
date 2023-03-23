namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteExpressionBlueprint : ICommandHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public DeleteExpressionBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            var expressionBlueprint = await this.db.ExpressionBlueprints
                                    .Include(x => x.InputDataVariableBlueprints)
                                    .Include(x => x.OutputDataVariableBlueprint)
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    .ConfigureAwait(false);

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            this.db.ExpressionBlueprints.Remove(expressionBlueprint);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}