namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class DeleteDataTableColumnBlueprint : ICommandHandler<Guid, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;

        public DeleteDataTableColumnBlueprint(BlueprintsWriteDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var dataTableColumnBlueprint = await this.db.DataTableColumnBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataTableColumnBlueprint is null)
            {
                throw new ArgumentException(nameof(dataTableColumnBlueprint));
            }

            this.db.DataTableColumnBlueprints.Remove(dataTableColumnBlueprint);
            await this.db.SaveChangesAsync(cancellation);
            return new DataTableColumnBlueprintDto(dataTableColumnBlueprint);
        }
    }
}