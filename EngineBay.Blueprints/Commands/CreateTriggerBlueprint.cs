namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTriggerBlueprint : ICommandHandler<TriggerBlueprint, TriggerBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<TriggerBlueprint> validator;

        public CreateTriggerBlueprint(BlueprintsWriteDbContext db, IValidator<TriggerBlueprint> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<TriggerBlueprintDto> Handle(TriggerBlueprint triggerBlueprint, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(triggerBlueprint);
            await this.db.TriggerBlueprints.AddAsync(triggerBlueprint, cancellation);
            await this.db.SaveChangesAsync(cancellation);
            return new TriggerBlueprintDto(triggerBlueprint);
        }
    }
}