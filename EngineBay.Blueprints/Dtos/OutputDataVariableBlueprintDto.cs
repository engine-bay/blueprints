namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class OutputDataVariableBlueprintDto : BaseDto
    {
        public OutputDataVariableBlueprintDto(OutputDataVariableBlueprint outputDataVariableBlueprint)
            : base(outputDataVariableBlueprint)
        {
            ArgumentNullException.ThrowIfNull(outputDataVariableBlueprint);

            this.ExpressionBlueprintId = outputDataVariableBlueprint.ExpressionBlueprintId;
            this.TriggerBlueprintId = outputDataVariableBlueprint.TriggerBlueprintId;
            this.Name = outputDataVariableBlueprint.Name;
            this.Namespace = outputDataVariableBlueprint.Namespace;
            this.Type = outputDataVariableBlueprint.Type;
        }

        public Guid? ExpressionBlueprintId { get; set; }

        public Guid? TriggerBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }
    }
}