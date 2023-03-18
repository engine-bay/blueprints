namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class BlueprintValidator : AbstractValidator<Blueprint>
    {
        public BlueprintValidator()
        {
            this.RuleFor(blueprint => blueprint.Name).NotNull().NotEmpty();
            this.RuleFor(blueprint => blueprint.Description);
            this.RuleForEach(blueprint => blueprint.ExpressionBlueprints).SetValidator(new ExpressionBlueprintValidator());
            this.RuleForEach(blueprint => blueprint.DataVariableBlueprints).SetValidator(new DataVariableBlueprintValidator());
            this.RuleForEach(blueprint => blueprint.DataTableBlueprints).SetValidator(new DataTableBlueprintValidator());
            this.RuleForEach(blueprint => blueprint.TriggerBlueprints).SetValidator(new TriggerBlueprintValidator());
        }
    }
}