namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class ExpressionBlueprintValidator : AbstractValidator<ExpressionBlueprint>
    {
        public ExpressionBlueprintValidator()
        {
            this.RuleFor(expressionBlueprint => expressionBlueprint.Expression).NotNull();
            this.RuleForEach(expressionBlueprint => expressionBlueprint.InputDataVariableBlueprints).SetValidator(new InputDataVariableBlueprintValidator());
            this.RuleForEach(expressionBlueprint => expressionBlueprint.InputDataTableBlueprints).SetValidator(new InputDataTableBlueprintValidator());
            this.RuleFor(expressionBlueprint => expressionBlueprint.OutputDataVariableBlueprint).NotNull().SetValidator(new OutputDataVariableBlueprintValidator());
        }
    }
}