namespace EngineBay.Blueprints
{
    using System;

    public class InputDataVariableBlueprintDto
    {
        public InputDataVariableBlueprintDto(InputDataVariableBlueprint inputDataVariableBlueprint)
        {
            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentNullException(nameof(inputDataVariableBlueprint));
            }

            this.Id = inputDataVariableBlueprint.Id;
            this.Name = inputDataVariableBlueprint.Name;
            this.Namespace = inputDataVariableBlueprint.Namespace;
            this.Type = inputDataVariableBlueprint.Type;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }
    }
}