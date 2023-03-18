namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class InputDataVariableBlueprintValidator : AbstractValidator<InputDataVariableBlueprint>
    {
        public InputDataVariableBlueprintValidator()
        {
            this.RuleFor(inputDataVariableBlueprint => inputDataVariableBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(inputDataVariableBlueprint => inputDataVariableBlueprint.Namespace).NotNull().NotEmpty();
            this.RuleFor(inputDataVariableBlueprint => inputDataVariableBlueprint.Type).NotNull().NotEmpty().Must(type =>
                type == DataVariableTypes.BOOL
                || type == DataVariableTypes.DATATABLE
                || type == DataVariableTypes.DATETIME
                || type == DataVariableTypes.FLOAT
                || type == DataVariableTypes.STRING);
        }
    }
}