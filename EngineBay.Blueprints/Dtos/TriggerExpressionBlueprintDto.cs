namespace EngineBay.Blueprints
{
    using System;

    public class TriggerExpressionBlueprintDto
    {
        public TriggerExpressionBlueprintDto(TriggerExpressionBlueprint triggerExpressionBlueprint)
        {
            if (triggerExpressionBlueprint is null)
            {
                throw new ArgumentNullException(nameof(triggerExpressionBlueprint));
            }

            this.Id = triggerExpressionBlueprint.Id;
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

        public Guid Id { get; set; }

        public Guid TriggerBlueprintId { get; set; }

        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public Guid InputDataVariableBlueprintId { get; set; }

        public InputDataVariableBlueprintDto? InputDataVariableBlueprint { get; set; }
    }
}