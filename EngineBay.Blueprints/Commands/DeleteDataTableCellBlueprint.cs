namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteDataTableCellBlueprint : ICommandHandler<Guid, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteDataTableCellBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var dataTableCellBlueprint = await this.db.DataTableCellBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableCellBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableCellBlueprint));
            }

            this.db.DataTableCellBlueprints.Remove(dataTableCellBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableCellBlueprintDto(dataTableCellBlueprint);
        }
    }
}