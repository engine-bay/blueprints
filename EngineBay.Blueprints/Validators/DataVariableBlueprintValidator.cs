namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class DataVariableBlueprintValidator : AbstractValidator<DataVariableBlueprint>
    {
        public DataVariableBlueprintValidator()
        {
            this.RuleFor(dataVariableBlueprint => dataVariableBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(dataVariableBlueprint => dataVariableBlueprint.Namespace).NotNull().NotEmpty();
            this.RuleFor(dataVariableBlueprint => dataVariableBlueprint.Type).NotNull().NotEmpty().Must(type =>
                type == DataVariableTypes.BOOL
                || type == DataVariableTypes.DATATABLE
                || type == DataVariableTypes.DATETIME
                || type == DataVariableTypes.FLOAT
                || type == DataVariableTypes.STRING);
        }
    }
}