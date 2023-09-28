namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetExpressionBlueprint : IQueryHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetExpressionBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.ExpressionBlueprints
                            .Include(x => x.InputDataTableBlueprints)
                            .Include(x => x.InputDataVariableBlueprints)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .Where(expressionBlueprint => expressionBlueprint.Id == id)
                            .Select(expressionBlueprint => new ExpressionBlueprintDto(expressionBlueprint))
                            .AsExpandable()
                            .FirstAsync(cancellation)
                            ;
        }
    }
}