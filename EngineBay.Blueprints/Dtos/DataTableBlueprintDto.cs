namespace EngineBay.Blueprints
{
    using System;

    public class DataTableBlueprintDto
    {
        public DataTableBlueprintDto(DataTableBlueprint dataTableBlueprint)
        {
            if (dataTableBlueprint is null)
            {
                throw new ArgumentNullException(nameof(dataTableBlueprint));
            }

            this.Id = dataTableBlueprint.Id;
            this.Name = dataTableBlueprint.Name;
            this.Namespace = dataTableBlueprint.Namespace;
            this.Description = dataTableBlueprint.Description;
            this.InputDataVariableBlueprints = dataTableBlueprint.InputDataVariableBlueprints?.Select(x => new InputDataVariableBlueprintDto(x)).ToList();
            this.DataTableColumnBlueprints = dataTableBlueprint.DataTableColumnBlueprints?.Select(x => new DataTableColumnBlueprintDto(x)).ToList();
            this.DataTableRowBlueprints = dataTableBlueprint.DataTableRowBlueprints?.Select(x => new DataTableRowBlueprintDto(x)).ToList();
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Namespace { get; set; }

        public IEnumerable<InputDataVariableBlueprintDto>? InputDataVariableBlueprints { get; set; }

        public IEnumerable<DataTableColumnBlueprintDto>? DataTableColumnBlueprints { get; set; }

        public IEnumerable<DataTableRowBlueprintDto>? DataTableRowBlueprints { get; set; }
    }
}