namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteExpressionBlueprint : ICommandHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var expressionBlueprint = await this.db.ExpressionBlueprints
                                    .Where(blueprint => blueprint.Id == id)
                                    .AsExpandable()
                                    .FirstAsync(cancellation)
                                    ;

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            this.db.ExpressionBlueprints.Remove(expressionBlueprint);
            await this.db.SaveChangesAsync(user, cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}