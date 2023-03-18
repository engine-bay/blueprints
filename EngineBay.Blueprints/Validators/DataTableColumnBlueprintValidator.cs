namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class DataTableColumnBlueprintValidator : AbstractValidator<DataTableColumnBlueprint>
    {
        public DataTableColumnBlueprintValidator()
        {
            this.RuleFor(dataTableColumnBlueprint => dataTableColumnBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(dataTableColumnBlueprint => dataTableColumnBlueprint.Type).NotNull().NotEmpty().Must(type =>
                type == DataVariableTypes.BOOL
                || type == DataVariableTypes.DATATABLE
                || type == DataVariableTypes.DATETIME
                || type == DataVariableTypes.FLOAT
                || type == DataVariableTypes.STRING);
        }
    }
}