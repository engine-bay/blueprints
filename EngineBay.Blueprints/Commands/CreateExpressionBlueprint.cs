namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateExpressionBlueprint : ICommandHandler<ExpressionBlueprint, ExpressionBlueprintDto>
    {
        private readonly BlueprintsEngineWriteDb db;
        private readonly IValidator<ExpressionBlueprint> validator;

        public CreateExpressionBlueprint(BlueprintsEngineWriteDb db, IValidator<ExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(ExpressionBlueprint expressionBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(expressionBlueprint);
            await this.db.ExpressionBlueprints.AddAsync(expressionBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}