namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class InputDataTableBlueprintValidator : AbstractValidator<InputDataTableBlueprint>
    {
        public InputDataTableBlueprintValidator()
        {
            this.RuleFor(inputDataTableBlueprint => inputDataTableBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(inputDataTableBlueprint => inputDataTableBlueprint.Namespace).NotNull().NotEmpty();
        }
    }
}