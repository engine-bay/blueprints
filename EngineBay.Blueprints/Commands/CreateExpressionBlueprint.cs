namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateExpressionBlueprint : ICommandHandler<ExpressionBlueprint, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public CreateExpressionBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(ExpressionBlueprint expressionBlueprint, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(expressionBlueprint);
            await this.db.ExpressionBlueprints.AddAsync(expressionBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}