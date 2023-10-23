namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetExpressionBlueprintMetaData : IQueryHandler<Guid, ExpressionBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetExpressionBlueprintMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<ExpressionBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.ExpressionBlueprints
                            .Where(expressionBlueprint => expressionBlueprint.Id == id)
                            .Select(expressionBlueprint => new ExpressionBlueprintDto(expressionBlueprint))
                            .AsExpandable()
                            .FirstAsync(cancellation)
                            ;
        }
    }
}