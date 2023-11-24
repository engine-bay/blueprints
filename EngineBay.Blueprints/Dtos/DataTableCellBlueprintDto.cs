namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class DataTableCellBlueprintDto : BaseDto
    {
        public DataTableCellBlueprintDto(DataTableCellBlueprint dataTableCellBlueprint)
            : base(dataTableCellBlueprint)
        {
            ArgumentNullException.ThrowIfNull(dataTableCellBlueprint);

            this.DataTableRowBlueprintId = dataTableCellBlueprint.DataTableRowBlueprintId;
            this.Key = dataTableCellBlueprint.Key;
            this.Value = dataTableCellBlueprint.Value;
            this.Name = dataTableCellBlueprint.Name;
            this.Namespace = dataTableCellBlueprint.Namespace;
        }

        public Guid? DataTableRowBlueprintId { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }
    }
}