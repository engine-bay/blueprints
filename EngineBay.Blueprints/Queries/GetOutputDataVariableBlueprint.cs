namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class GetOutputDataVariableBlueprint : IQueryHandler<Guid, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetOutputDataVariableBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var outputDataVariableBlueprint = await this.db.OutputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(outputDataVariableBlueprint));
            }

            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}