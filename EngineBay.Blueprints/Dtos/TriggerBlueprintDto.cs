namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class TriggerBlueprintDto : BaseDto
    {
        public TriggerBlueprintDto(TriggerBlueprint triggerBlueprint)
            : base(triggerBlueprint)
        {
            ArgumentNullException.ThrowIfNull(triggerBlueprint);

            this.BlueprintId = triggerBlueprint.BlueprintId;
            this.Name = triggerBlueprint.Name;
            this.Description = triggerBlueprint.Description;
            this.TriggerExpressionBlueprints = triggerBlueprint.TriggerExpressionBlueprints?.Select(x => new TriggerExpressionBlueprintDto(x)).ToList();
            this.OutputDataVariableBlueprintId = triggerBlueprint.OutputDataVariableBlueprintId;
            if (triggerBlueprint.OutputDataVariableBlueprint is not null)
            {
                this.OutputDataVariableBlueprint = new OutputDataVariableBlueprintDto(triggerBlueprint.OutputDataVariableBlueprint);
            }
        }

        public Guid? BlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TriggerExpressionBlueprintDto>? TriggerExpressionBlueprints { get; set; }

        public Guid? OutputDataVariableBlueprintId { get; set; }

        public OutputDataVariableBlueprintDto? OutputDataVariableBlueprint { get; set; }
    }
}