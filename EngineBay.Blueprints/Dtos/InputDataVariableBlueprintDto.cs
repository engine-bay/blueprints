namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class InputDataVariableBlueprintDto : BaseDto
    {
        public InputDataVariableBlueprintDto(InputDataVariableBlueprint inputDataVariableBlueprint)
            : base(inputDataVariableBlueprint)
        {
            ArgumentNullException.ThrowIfNull(inputDataVariableBlueprint);

            this.ExpressionBlueprintId = inputDataVariableBlueprint.ExpressionBlueprintId;
            this.DataTableBlueprintId = inputDataVariableBlueprint.DataTableBlueprintId;
            this.TriggerExpressionBlueprintId = inputDataVariableBlueprint.TriggerExpressionBlueprintId;
            this.Name = inputDataVariableBlueprint.Name;
            this.Namespace = inputDataVariableBlueprint.Namespace;
            this.Type = inputDataVariableBlueprint.Type;
        }

        public Guid? ExpressionBlueprintId { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public Guid? TriggerExpressionBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }
    }
}