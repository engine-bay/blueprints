namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class UpdateExpressionBlueprint : ICommandHandler<UpdateParameters<ExpressionBlueprint>, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public UpdateExpressionBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(UpdateParameters<ExpressionBlueprint> updateParameters, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
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

            var expressionBlueprint = await this.db.ExpressionBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            expressionBlueprint.Expression = updateExpressionBlueprint.Expression;
            expressionBlueprint.Objective = updateExpressionBlueprint.Objective;

            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}