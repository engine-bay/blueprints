namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteOutputDataVariableBlueprint : ICommandHandler<Guid, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteOutputDataVariableBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var outputDataVariableBlueprint = await this.db.OutputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(outputDataVariableBlueprint));
            }

            this.db.OutputDataVariableBlueprints.Remove(outputDataVariableBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}