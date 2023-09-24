namespace EngineBay.Blueprints
{
    using System;

    public class ExpressionBlueprintDto : ExpressionBlueprintMetaDataDto
    {
        public ExpressionBlueprintDto(ExpressionBlueprint expressionBlueprint)
            : base(expressionBlueprint)
        {
            if (expressionBlueprint is null)
            {
                throw new ArgumentNullException(nameof(expressionBlueprint));
            }

            this.InputDataVariableBlueprints = expressionBlueprint.InputDataVariableBlueprints?.Select(x => new InputDataVariableBlueprintDto(x)).ToList();
            this.InputDataTableBlueprints = expressionBlueprint.InputDataTableBlueprints?.Select(x => new InputDataTableBlueprintDto(x)).ToList();

            if (expressionBlueprint.OutputDataVariableBlueprint is not null)
            {
                this.OutputDataVariableBlueprint = new OutputDataVariableBlueprintDto(expressionBlueprint.OutputDataVariableBlueprint);
            }
        }

        public IEnumerable<InputDataVariableBlueprintDto>? InputDataVariableBlueprints { get; set; }

        public IEnumerable<InputDataTableBlueprintDto>? InputDataTableBlueprints { get; set; }

        public OutputDataVariableBlueprintDto? OutputDataVariableBlueprint { get; set; }
    }
}