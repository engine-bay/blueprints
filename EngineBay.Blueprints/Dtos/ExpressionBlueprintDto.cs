namespace EngineBay.Blueprints
{
    using System;

    public class ExpressionBlueprintDto
    {
        public ExpressionBlueprintDto(ExpressionBlueprint expressionBlueprint)
        {
            if (expressionBlueprint is null)
            {
                throw new ArgumentNullException(nameof(expressionBlueprint));
            }

            this.Id = expressionBlueprint.Id;
            this.BlueprintId = expressionBlueprint.BlueprintId;
            this.Expression = expressionBlueprint.Expression;
            this.Objective = expressionBlueprint.Objective;
            this.InputDataVariableBlueprints = expressionBlueprint.InputDataVariableBlueprints?.Select(x => new InputDataVariableBlueprintDto(x)).ToList();
            this.InputDataTableBlueprints = expressionBlueprint.InputDataTableBlueprints?.Select(x => new InputDataTableBlueprintDto(x)).ToList();
            this.OutputDataVariableId = expressionBlueprint.OutputDataVariableBlueprintId;

            if (expressionBlueprint.OutputDataVariableBlueprint is not null)
            {
                this.OutputDataVariableBlueprint = new OutputDataVariableBlueprintDto(expressionBlueprint.OutputDataVariableBlueprint);
            }
        }

        public Guid Id { get; set; }

        public Guid? BlueprintId { get; set; }

        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public IEnumerable<InputDataVariableBlueprintDto>? InputDataVariableBlueprints { get; set; }

        public IEnumerable<InputDataTableBlueprintDto>? InputDataTableBlueprints { get; set; }

        public Guid? OutputDataVariableId { get; set; }

        public OutputDataVariableBlueprintDto? OutputDataVariableBlueprint { get; set; }
    }
}