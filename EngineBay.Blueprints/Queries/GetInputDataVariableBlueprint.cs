namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class GetInputDataVariableBlueprint : IQueryHandler<Guid, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetInputDataVariableBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var inputDataVariableBlueprint = await this.db.InputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(inputDataVariableBlueprint));
            }

            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}