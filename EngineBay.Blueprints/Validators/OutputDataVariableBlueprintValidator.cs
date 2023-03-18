namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class OutputDataVariableBlueprintValidator : AbstractValidator<OutputDataVariableBlueprint>
    {
        public OutputDataVariableBlueprintValidator()
        {
            this.RuleFor(outputDataVariableBlueprint => outputDataVariableBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(outputDataVariableBlueprint => outputDataVariableBlueprint.Namespace).NotNull().NotEmpty();
            this.RuleFor(outputDataVariableBlueprint => outputDataVariableBlueprint.Type).NotNull().NotEmpty().Must(type =>
                type == DataVariableTypes.BOOL
                || type == DataVariableTypes.DATATABLE
                || type == DataVariableTypes.DATETIME
                || type == DataVariableTypes.FLOAT
                || type == DataVariableTypes.STRING);
        }
    }
}