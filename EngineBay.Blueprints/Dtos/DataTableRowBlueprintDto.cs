namespace EngineBay.Blueprints
{
    using System;

    public class DataTableRowBlueprintDto
    {
        public DataTableRowBlueprintDto(DataTableRowBlueprint dataTableRowBlueprint)
        {
            if (dataTableRowBlueprint is null)
            {
                throw new ArgumentNullException(nameof(dataTableRowBlueprint));
            }

            this.Id = dataTableRowBlueprint.Id;
            this.DataTableBlueprintId = dataTableRowBlueprint.DataTableBlueprintId;

            this.DataTableCellBlueprints = dataTableRowBlueprint.DataTableCellBlueprints?.Select(x => new DataTableCellBlueprintDto(x)).ToList();
        }

        public Guid Id { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public IEnumerable<DataTableCellBlueprintDto>? DataTableCellBlueprints { get; set; }
    }
}