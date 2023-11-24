namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateTriggerExpressionBlueprint : ICommandHandler<UpdateParameters<TriggerExpressionBlueprint>, TriggerExpressionBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerExpressionBlueprint> validator;

        public UpdateTriggerExpressionBlueprint(BlueprintsWriteDbContext db, IValidator<TriggerExpressionBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerExpressionBlueprintDto> Handle(UpdateParameters<TriggerExpressionBlueprint> updateParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(updateParameters);

            var id = updateParameters.Id;
            var updateTriggerExpressionBlueprint = updateParameters.Entity;

            if (updateTriggerExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(updateTriggerExpressionBlueprint));
            }

            this.validator.ValidateAndThrow(updateTriggerExpressionBlueprint);

            var triggerExpressionBlueprint = await this.db.TriggerExpressionBlueprints.FindAsync(new object[] { id }, cancellation);

            if (triggerExpressionBlueprint is null)
            {
                throw new ArgumentException(nameof(triggerExpressionBlueprint));
            }

            triggerExpressionBlueprint.Expression = updateTriggerExpressionBlueprint.Expression;
            triggerExpressionBlueprint.Objective = updateTriggerExpressionBlueprint.Objective;

            await this.db.SaveChangesAsync(cancellation);
            return new TriggerExpressionBlueprintDto(triggerExpressionBlueprint);
        }
    }
}