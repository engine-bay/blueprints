namespace EngineBay.Blueprints
{
    using System;

    public class BlueprintDto : BlueprintMetaDataDto
    {
        public BlueprintDto(Blueprint blueprint)
            : base(blueprint)
        {
            if (blueprint is null)
            {
                throw new ArgumentNullException(nameof(blueprint));
            }

            this.ExpressionBlueprints = blueprint.ExpressionBlueprints?.Select(x => new ExpressionBlueprintDto(x)).ToList();
            this.DataVariableBlueprints = blueprint.DataVariableBlueprints?.Select(x => new DataVariableBlueprintDto(x)).ToList();
            this.DataTableBlueprints = blueprint.DataTableBlueprints?.Select(x => new DataTableBlueprintDto(x)).ToList();
            this.TriggerBlueprints = blueprint.TriggerBlueprints?.Select(x => new TriggerBlueprintDto(x)).ToList();
        }

        public IEnumerable<ExpressionBlueprintDto>? ExpressionBlueprints { get; set; }

        public IEnumerable<DataVariableBlueprintDto>? DataVariableBlueprints { get; set; }

        public IEnumerable<DataTableBlueprintDto>? DataTableBlueprints { get; set; }

        public IEnumerable<TriggerBlueprintDto>? TriggerBlueprints { get; set; }
    }
}