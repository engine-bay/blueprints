namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class DataTableCellBlueprintValidator : AbstractValidator<DataTableCellBlueprint>
    {
        public DataTableCellBlueprintValidator()
        {
            this.RuleFor(dataTableCellBlueprint => dataTableCellBlueprint.Key).NotNull().NotEmpty();
            this.RuleFor(dataTableCellBlueprint => dataTableCellBlueprint.Key).NotNull();
            this.RuleFor(dataTableCellBlueprint => dataTableCellBlueprint.Name).NotNull().NotEmpty();
            this.RuleFor(dataTableCellBlueprint => dataTableCellBlueprint.Namespace).NotNull().NotEmpty();
        }
    }
}