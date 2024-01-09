namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class DataTableColumnBlueprintDto : BaseDto
    {
        public DataTableColumnBlueprintDto(DataTableColumnBlueprint dataTableColumnBlueprint)
            : base(dataTableColumnBlueprint)
        {
            ArgumentNullException.ThrowIfNull(dataTableColumnBlueprint);

            this.DataTableBlueprintId = dataTableColumnBlueprint.DataTableBlueprintId;
            this.Name = dataTableColumnBlueprint.Name;
            this.Type = dataTableColumnBlueprint.Type;
        }

        public Guid? DataTableBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }
    }
}