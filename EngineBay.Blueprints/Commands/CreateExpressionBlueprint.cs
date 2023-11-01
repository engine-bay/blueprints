namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateExpressionBlueprint : ICommandHandler<ExpressionBlueprint, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        public CreateExpressionBlueprint(BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(ExpressionBlueprint expressionBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(expressionBlueprint);
            await this.db.ExpressionBlueprints.AddAsync(expressionBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}