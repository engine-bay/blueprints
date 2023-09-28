namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class GetDataVariableBlueprint : IQueryHandler<Guid, DataVariableBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetDataVariableBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}