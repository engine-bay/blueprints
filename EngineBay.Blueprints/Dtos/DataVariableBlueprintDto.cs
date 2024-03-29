namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class DataVariableBlueprintDto : BaseDto
    {
        public DataVariableBlueprintDto(DataVariableBlueprint dataVariableBlueprint)
            : base(dataVariableBlueprint)
        {
            ArgumentNullException.ThrowIfNull(dataVariableBlueprint);

            this.BlueprintId = dataVariableBlueprint.BlueprintId;
            this.Name = dataVariableBlueprint.Name;
            this.Description = dataVariableBlueprint.Description;
            this.Type = dataVariableBlueprint.Type;
            this.Namespace = dataVariableBlueprint.Namespace;
            this.DefaultValue = dataVariableBlueprint.DefaultValue;
        }

        public Guid? BlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public string? Namespace { get; set; }

        public string? DefaultValue { get; set; }
    }
}