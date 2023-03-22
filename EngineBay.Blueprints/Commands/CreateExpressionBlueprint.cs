namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateExpressionBlueprint : ICommandHandler<ExpressionBlueprint, ApplicationUser, ExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<ExpressionBlueprint> validator;

        public CreateExpressionBlueprint(BlueprintsWriteDbContext db, IValidator<ExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(ExpressionBlueprint expressionBlueprint, ApplicationUser user, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(expressionBlueprint);
            await this.db.ExpressionBlueprints.AddAsync(expressionBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}