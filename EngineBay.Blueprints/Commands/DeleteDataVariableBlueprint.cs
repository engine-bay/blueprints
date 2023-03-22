namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public class DeleteDataVariableBlueprint : ICommandHandler<Guid, ApplicationUser, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteDataVariableBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(Guid id, ApplicationUser user, CancellationToken cancellation)
        {
            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            this.db.DataVariableBlueprints.Remove(dataVariableBlueprint);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}