namespace EngineBay.Blueprints
{
    using System;

    public class InputDataTableBlueprintDto
    {
        public InputDataTableBlueprintDto(InputDataTableBlueprint inputDataTableBlueprint)
        {
            if (inputDataTableBlueprint is null)
            {
                throw new ArgumentNullException(nameof(inputDataTableBlueprint));
            }

            this.Id = inputDataTableBlueprint.Id;
            this.ExpressionBlueprintId = inputDataTableBlueprint.ExpressionBlueprintId;
            this.Name = inputDataTableBlueprint.Name;
            this.Namespace = inputDataTableBlueprint.Namespace;
        }

        public Guid Id { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }
    }
}