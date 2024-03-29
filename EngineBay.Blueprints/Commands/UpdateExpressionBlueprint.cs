namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateExpressionBlueprint : ICommandHandler<UpdateParameters<ExpressionBlueprint>, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        public UpdateExpressionBlueprint(BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(UpdateParameters<ExpressionBlueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

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

            await this.db.SaveChangesAsync(cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}