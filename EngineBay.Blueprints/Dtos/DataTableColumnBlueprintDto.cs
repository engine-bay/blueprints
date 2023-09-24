namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class DataTableColumnBlueprintDto : BaseDto
    {
        public DataTableColumnBlueprintDto(DataTableColumnBlueprint dataTableColumnBlueprint)
            : base(dataTableColumnBlueprint)
        {
            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentNullException(nameof(dataTableColumnBlueprint));
            }

            this.DataTableBlueprintId = dataTableColumnBlueprint.DataTableBlueprintId;
            this.Name = dataTableColumnBlueprint.Name;
            this.Type = dataTableColumnBlueprint.Type;
        }

        public Guid? DataTableBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }
    }
}