namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateExpressionBlueprint : ICommandHandler<ExpressionBlueprint, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(ExpressionBlueprint expressionBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(expressionBlueprint);
            await this.db.ExpressionBlueprints.AddAsync(expressionBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}