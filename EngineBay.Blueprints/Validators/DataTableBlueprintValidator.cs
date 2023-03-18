namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class DataTableBlueprintValidator : AbstractValidator<DataTableBlueprint>
    {
        public DataTableBlueprintValidator()
        {
            this.RuleFor(dataTableBlueprint => dataTableBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(dataTableBlueprint => dataTableBlueprint.Namespace).NotNull().NotEmpty();
            this.RuleForEach(dataTableBlueprint => dataTableBlueprint.InputDataVariableBlueprints).SetValidator(new InputDataVariableBlueprintValidator());
            this.RuleForEach(dataTableBlueprint => dataTableBlueprint.DataTableColumnBlueprints).SetValidator(new DataTableColumnBlueprintValidator());
            this.RuleForEach(dataTableBlueprint => dataTableBlueprint.DataTableRowBlueprints).SetValidator(new DataTableRowBlueprintValidator());
        }
    }
}