namespace EngineBay.Blueprints
{
    using System;

    public class OutputDataVariableBlueprintDto
    {
        public OutputDataVariableBlueprintDto(OutputDataVariableBlueprint outputDataVariableBlueprint)
        {
            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentNullException(nameof(outputDataVariableBlueprint));
            }

            this.Id = outputDataVariableBlueprint.Id;
            this.ExpressionBlueprintId = outputDataVariableBlueprint.ExpressionBlueprintId;
            this.TriggerBlueprintId = outputDataVariableBlueprint.TriggerBlueprintId;
            this.Name = outputDataVariableBlueprint.Name;
            this.Namespace = outputDataVariableBlueprint.Namespace;
            this.Type = outputDataVariableBlueprint.Type;
        }

        public Guid Id { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public Guid? TriggerBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }
    }
}