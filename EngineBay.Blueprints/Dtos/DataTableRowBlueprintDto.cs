namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class DataTableRowBlueprintDto : BaseDto
    {
        public DataTableRowBlueprintDto(DataTableRowBlueprint dataTableRowBlueprint)
            : base(dataTableRowBlueprint)
        {
            ArgumentNullException.ThrowIfNull(dataTableRowBlueprint);

            this.DataTableBlueprintId = dataTableRowBlueprint.DataTableBlueprintId;

            this.DataTableCellBlueprints = dataTableRowBlueprint.DataTableCellBlueprints?.Select(x => new DataTableCellBlueprintDto(x)).ToList();
        }

        public Guid? DataTableBlueprintId { get; set; }

        public IEnumerable<DataTableCellBlueprintDto>? DataTableCellBlueprints { get; set; }
    }
}