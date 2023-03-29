namespace EngineBay.Blueprints
{
    using System;

    public class BlueprintDto
    {
        public BlueprintDto(Blueprint blueprint)
        {
            if (blueprint is null)
            {
                throw new ArgumentNullException(nameof(blueprint));
            }

            this.Id = blueprint.Id;
            this.WorkbookId = blueprint.WorkbookId;
            this.Name = blueprint.Name;
            this.Description = blueprint.Description;
            this.ExpressionBlueprints = blueprint.ExpressionBlueprints?.Select(x => new ExpressionBlueprintDto(x)).ToList();
            this.DataVariableBlueprints = blueprint.DataVariableBlueprints?.Select(x => new DataVariableBlueprintDto(x)).ToList();
            this.DataTableBlueprints = blueprint.DataTableBlueprints?.Select(x => new DataTableBlueprintDto(x)).ToList();
            this.TriggerBlueprints = blueprint.TriggerBlueprints?.Select(x => new TriggerBlueprintDto(x)).ToList();
        }

        public Guid Id { get; set; }

        public Guid? WorkbookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<ExpressionBlueprintDto>? ExpressionBlueprints { get; set; }

        public IEnumerable<DataVariableBlueprintDto>? DataVariableBlueprints { get; set; }

        public IEnumerable<DataTableBlueprintDto>? DataTableBlueprints { get; set; }

        public IEnumerable<TriggerBlueprintDto>? TriggerBlueprints { get; set; }
    }
}