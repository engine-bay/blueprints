namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteDataTableBlueprint : ICommandHandler<Guid, DataTableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteDataTableBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var dataTableBlueprint = await this.db.DataTableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableBlueprint));
            }

            this.db.DataTableBlueprints.Remove(dataTableBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableBlueprintDto(dataTableBlueprint);
        }
    }
}