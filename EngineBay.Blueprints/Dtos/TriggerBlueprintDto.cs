namespace EngineBay.Blueprints
{
    using System;

    public class TriggerBlueprintDto
    {
        public TriggerBlueprintDto(TriggerBlueprint triggerBlueprint)
        {
            if (triggerBlueprint is null)
            {
                throw new ArgumentNullException(nameof(triggerBlueprint));
            }

            this.Id = triggerBlueprint.Id;
            this.Name = triggerBlueprint.Name;
            this.Description = triggerBlueprint.Description;
            this.TriggerExpressionBlueprints = triggerBlueprint.TriggerExpressionBlueprints?.Select(x => new TriggerExpressionBlueprintDto(x)).ToList();
            this.OutputDataVariableBlueprintId = triggerBlueprint.OutputDataVariableBlueprintId;
            if (triggerBlueprint.OutputDataVariableBlueprint is not null)
            {
                this.OutputDataVariableBlueprint = new OutputDataVariableBlueprintDto(triggerBlueprint.OutputDataVariableBlueprint);
            }
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TriggerExpressionBlueprintDto>? TriggerExpressionBlueprints { get; set; }

        public Guid OutputDataVariableBlueprintId { get; set; }

        public OutputDataVariableBlueprintDto? OutputDataVariableBlueprint { get; set; }
    }
}