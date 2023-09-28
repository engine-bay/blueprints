namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateExpressionBlueprint : ICommandHandler<UpdateParameters<ExpressionBlueprint>, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateExpressionBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(UpdateParameters<ExpressionBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateExpressionBlueprint = updateParameters.Entity;

            if (updateExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(updateExpressionBlueprint));
            }

            this.validator.ValidateAndThrow(updateExpressionBlueprint);

            var expressionBlueprint = await this.db.ExpressionBlueprints.FindAsync(new object[] { id }, cancellation);

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            expressionBlueprint.Expression = updateExpressionBlueprint.Expression;
            expressionBlueprint.Objective = updateExpressionBlueprint.Objective;

            await this.db.SaveChangesAsync(user, cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}