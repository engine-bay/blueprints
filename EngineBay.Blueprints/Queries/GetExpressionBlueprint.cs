namespace EngineBay.Blueprints
{
    using EngineBay.Core;

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
            var expressionBlueprint = await this.db.ExpressionBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (expressionBlueprint is null)
            {
                throw new ArgumentException(nameof(expressionBlueprint));
            }

            return new ExpressionBlueprintDto(expressionBlueprint);
        }
    }
}