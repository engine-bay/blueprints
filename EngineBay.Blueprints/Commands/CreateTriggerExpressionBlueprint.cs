namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTriggerExpressionBlueprint : ICommandHandler<TriggerExpressionBlueprint, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerExpressionBlueprint> validator;

        public CreateTriggerExpressionBlueprint(BlueprintsWriteDbContext db, IValidator<TriggerExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(TriggerExpressionBlueprint triggerExpressionBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(triggerExpressionBlueprint);
            await this.db.TriggerExpressionBlueprints.AddAsync(triggerExpressionBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}