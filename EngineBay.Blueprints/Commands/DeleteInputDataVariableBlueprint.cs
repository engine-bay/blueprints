namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteInputDataVariableBlueprint : ICommandHandler<Guid, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteInputDataVariableBlueprint(BlueprintsWriteDbContext db)
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

            this.db.InputDataVariableBlueprints.Remove(inputDataVariableBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}