namespace EngineBay.Blueprints
{
    using System;

    public class DataTableColumnBlueprintDto
    {
        public DataTableColumnBlueprintDto(DataTableColumnBlueprint dataTableColumnBlueprint)
        {
            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentNullException(nameof(dataTableColumnBlueprint));
            }

            this.Id = dataTableColumnBlueprint.Id;
            this.Name = dataTableColumnBlueprint.Name;
            this.Type = dataTableColumnBlueprint.Type;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }
    }
}