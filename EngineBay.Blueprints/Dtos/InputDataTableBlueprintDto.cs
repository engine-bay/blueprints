namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;

    public class InputDataTableBlueprintDto : BaseDto
    {
        public InputDataTableBlueprintDto(InputDataTableBlueprint inputDataTableBlueprint)
            : base(inputDataTableBlueprint)
        {
            ArgumentNullException.ThrowIfNull(inputDataTableBlueprint);

            this.ExpressionBlueprintId = inputDataTableBlueprint.ExpressionBlueprintId;
            this.Name = inputDataTableBlueprint.Name;
            this.Namespace = inputDataTableBlueprint.Namespace;
        }

        public Guid? ExpressionBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }
    }
}