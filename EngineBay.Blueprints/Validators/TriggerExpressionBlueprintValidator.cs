namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class TriggerExpressionBlueprintValidator : AbstractValidator<TriggerExpressionBlueprint>
    {
        public TriggerExpressionBlueprintValidator()
        {
            this.RuleFor(triggerExpressionBlueprint => triggerExpressionBlueprint.Expression).NotNull();
            this.RuleFor(triggerExpressionBlueprint => triggerExpressionBlueprint.InputDataVariableBlueprint).NotNull();
        }
    }
}