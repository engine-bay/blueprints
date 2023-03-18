namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class TriggerBlueprintValidator : AbstractValidator<TriggerBlueprint>
    {
        public TriggerBlueprintValidator()
        {
            this.RuleFor(triggerBlueprint => triggerBlueprint.Name).NotNull().NotEmpty();
            this.RuleForEach(triggerBlueprint => triggerBlueprint.TriggerExpressionBlueprints).SetValidator(new TriggerExpressionBlueprintValidator());
            this.RuleFor(triggerBlueprint => triggerBlueprint.OutputDataVariableBlueprint).NotNull();
        }
    }
}