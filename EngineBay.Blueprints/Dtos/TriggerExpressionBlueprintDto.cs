namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class TriggerExpressionBlueprintDto : BaseDto
    {
        public TriggerExpressionBlueprintDto(TriggerExpressionBlueprint triggerExpressionBlueprint)
            : base(triggerExpressionBlueprint)
        {
            ArgumentNullException.ThrowIfNull(triggerExpressionBlueprint);

            this.TriggerBlueprintId = triggerExpressionBlueprint.TriggerBlueprintId;
            this.Expression = triggerExpressionBlueprint.Expression;
            this.Objective = triggerExpressionBlueprint.Objective;
            this.Objective = triggerExpressionBlueprint.Objective;
            this.InputDataVariableBlueprintId = triggerExpressionBlueprint.InputDataVariableBlueprintId;
            if (triggerExpressionBlueprint.InputDataVariableBlueprint is not null)
            {
                this.InputDataVariableBlueprint = new InputDataVariableBlueprintDto(triggerExpressionBlueprint.InputDataVariableBlueprint);
            }
        }

        public Guid? TriggerBlueprintId { get; set; }

        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public Guid? InputDataVariableBlueprintId { get; set; }

        public InputDataVariableBlueprintDto? InputDataVariableBlueprint { get; set; }
    }
}