namespace EngineBay.Blueprints
{
    using System;

    public class InputDataVariableBlueprintDto
    {
        public InputDataVariableBlueprintDto(InputDataVariableBlueprint inputDataVariableBlueprint)
        {
            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentNullException(nameof(inputDataVariableBlueprint));
            }

            this.Id = inputDataVariableBlueprint.Id;
            this.ExpressionBlueprintId = inputDataVariableBlueprint.ExpressionBlueprintId;
            this.DataTableBlueprintId = inputDataVariableBlueprint.DataTableBlueprintId;
            this.TriggerExpressionBlueprintId = inputDataVariableBlueprint.TriggerExpressionBlueprintId;
            this.Name = inputDataVariableBlueprint.Name;
            this.Namespace = inputDataVariableBlueprint.Namespace;
            this.Type = inputDataVariableBlueprint.Type;
        }

        public Guid Id { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public Guid? TriggerExpressionBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }
    }
}