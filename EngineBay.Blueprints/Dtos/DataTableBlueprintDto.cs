namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class DataTableBlueprintDto : BaseDto
    {
        public DataTableBlueprintDto(DataTableBlueprint dataTableBlueprint)
            : base(dataTableBlueprint)
        {
            ArgumentNullException.ThrowIfNull(dataTableBlueprint);

            this.BlueprintId = dataTableBlueprint.BlueprintId;
            this.Name = dataTableBlueprint.Name;
            this.Namespace = dataTableBlueprint.Namespace;
            this.Description = dataTableBlueprint.Description;
            this.InputDataVariableBlueprints = dataTableBlueprint.InputDataVariableBlueprints?.Select(x => new InputDataVariableBlueprintDto(x)).ToList();
            this.DataTableColumnBlueprints = dataTableBlueprint.DataTableColumnBlueprints?.Select(x => new DataTableColumnBlueprintDto(x)).ToList();
            this.DataTableRowBlueprints = dataTableBlueprint.DataTableRowBlueprints?.Select(x => new DataTableRowBlueprintDto(x)).ToList();
        }

        public Guid? BlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Namespace { get; set; }

        public IEnumerable<InputDataVariableBlueprintDto>? InputDataVariableBlueprints { get; set; }

        public IEnumerable<DataTableColumnBlueprintDto>? DataTableColumnBlueprints { get; set; }

        public IEnumerable<DataTableRowBlueprintDto>? DataTableRowBlueprints { get; set; }
    }
}