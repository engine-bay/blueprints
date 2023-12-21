namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class ExpressionBlueprintMetaDataDto : BaseDto
    {
        public ExpressionBlueprintMetaDataDto(ExpressionBlueprint expressionBlueprint)
            : base(expressionBlueprint)
        {
            ArgumentNullException.ThrowIfNull(expressionBlueprint);

            this.BlueprintId = expressionBlueprint.BlueprintId;
            this.Expression = expressionBlueprint.Expression;
            this.Objective = expressionBlueprint.Objective;
            this.OutputDataVariableId = expressionBlueprint.OutputDataVariableBlueprintId;
        }

        public Guid? BlueprintId { get; set; }

        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public Guid? OutputDataVariableId { get; set; }
    }
}