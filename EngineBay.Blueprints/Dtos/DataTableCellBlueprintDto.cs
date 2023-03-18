namespace EngineBay.Blueprints
{
    using System;

    public class DataTableCellBlueprintDto
    {
        public DataTableCellBlueprintDto(DataTableCellBlueprint dataTableCellBlueprint)
        {
            if (dataTableCellBlueprint is null)
            {
                throw new ArgumentNullException(nameof(dataTableCellBlueprint));
            }

            this.Id = dataTableCellBlueprint.Id;
            this.Key = dataTableCellBlueprint.Key;
            this.Value = dataTableCellBlueprint.Value;
            this.Name = dataTableCellBlueprint.Name;
            this.Namespace = dataTableCellBlueprint.Namespace;
        }

        public Guid Id { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }
    }
}