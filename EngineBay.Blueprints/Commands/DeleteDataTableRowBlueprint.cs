namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteDataTableRowBlueprint : ICommandHandler<Guid, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteDataTableRowBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var dataTableRowBlueprint = await this.db.DataTableRowBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableRowBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableRowBlueprint));
            }

            this.db.DataTableRowBlueprints.Remove(dataTableRowBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableRowBlueprintDto(dataTableRowBlueprint);
        }
    }
}