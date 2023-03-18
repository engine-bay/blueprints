namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class DataTableRowBlueprintValidator : AbstractValidator<DataTableRowBlueprint>
    {
        public DataTableRowBlueprintValidator()
        {
            this.RuleForEach(dataTableRowBlueprint => dataTableRowBlueprint.DataTableCellBlueprints).SetValidator(new DataTableCellBlueprintValidator());
        }
    }
}