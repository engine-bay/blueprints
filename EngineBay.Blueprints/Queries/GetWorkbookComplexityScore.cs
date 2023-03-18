namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class GetWorkbookComplexityScore : IQueryHandler<Guid, WorkbookComplexityScoreDto>
    {
        private readonly BlueprintsEngineQueryDb db;
        private readonly GetWorkbook query;

        public GetWorkbookComplexityScore(BlueprintsEngineQueryDb db, GetWorkbook query)
        {
            this.db = db;
            this.query = query;
        }

        /// <inheritdoc/>
        public async Task<WorkbookComplexityScoreDto> Handle(Guid id, CancellationToken cancellation)
        {
            var workbook = await this.query.Handle(id, cancellation).ConfigureAwait(false);

            if (workbook is null)
            {
                throw new ArgumentException(nameof(workbook));
            }

            if (workbook.Blueprints is null)
            {
                throw new ArgumentException(nameof(workbook.Blueprints));
            }

            double totalExpressions = 0;
            double totalVariables = 0;
            double totalLinks = 0;

            // Todo some future graph complexity measures: https://stats.stackexchange.com/questions/239652/measure-graph-complexity
            foreach (var blueprint in workbook.Blueprints)
            {
                if (blueprint.ExpressionBlueprints is null)
                {
                    throw new ArgumentException(nameof(blueprint.ExpressionBlueprints));
                }

                totalExpressions += blueprint.ExpressionBlueprints.Count();

                if (blueprint.DataVariableBlueprints is null)
                {
                    throw new ArgumentException(nameof(blueprint.DataVariableBlueprints));
                }

                totalVariables += blueprint.DataVariableBlueprints.Count();

                foreach (var expressionBlueprint in blueprint.ExpressionBlueprints)
                {
                    if (expressionBlueprint.InputDataVariableBlueprints is null)
                    {
                        throw new ArgumentException(nameof(expressionBlueprint.InputDataVariableBlueprints));
                    }

                    totalLinks += expressionBlueprint.InputDataVariableBlueprints.Count(); // all of the links to input data variable dependencies
                    totalLinks += 1; // One for the output data variable
                }

                if (blueprint.TriggerBlueprints is null)
                {
                    throw new ArgumentException(nameof(blueprint.TriggerBlueprints));
                }

                foreach (var triggerBlueprints in blueprint.TriggerBlueprints)
                {
                    if (triggerBlueprints.TriggerExpressionBlueprints is null)
                    {
                        throw new ArgumentException(nameof(triggerBlueprints.TriggerExpressionBlueprints));
                    }

                    totalExpressions += triggerBlueprints.TriggerExpressionBlueprints.Count();
                    totalLinks += triggerBlueprints.TriggerExpressionBlueprints.Count(); // Each trigger has one input data variable dependency, therefore one link
                    totalLinks += 1; // One for the output data variable of the trigger
                }

                if (blueprint.DataTableBlueprints is null)
                {
                    throw new ArgumentException(nameof(blueprint.DataTableBlueprints));
                }

                totalVariables += blueprint.DataTableBlueprints.Count();

                foreach (var dataTableBlueprint in blueprint.DataTableBlueprints)
                {
                    if (dataTableBlueprint.DataTableRowBlueprints is null)
                    {
                        throw new ArgumentException(nameof(dataTableBlueprint.DataTableRowBlueprints));
                    }

                    foreach (var dataTableRowBlueprint in dataTableBlueprint.DataTableRowBlueprints)
                    {
                        if (dataTableRowBlueprint.DataTableCellBlueprints is null)
                        {
                            throw new ArgumentException(nameof(dataTableRowBlueprint.DataTableCellBlueprints));
                        }

                        totalVariables += dataTableRowBlueprint.DataTableCellBlueprints.Count();
                    }
                }
            }

            double score = (totalExpressions / totalVariables) * totalLinks;

            return new WorkbookComplexityScoreDto
            {
                WorkbookId = workbook.Id,
                TotalExpressions = totalExpressions,
                TotalVariables = totalVariables,
                TotalLinks = totalLinks,
                Score = score,
            };
        }
    }
}