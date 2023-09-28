namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetDataTableCellBlueprint : IQueryHandler<Guid, DataTableCellBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetDataTableCellBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableCellBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.DataTableCellBlueprints
                        .Where(x => x.Id == id)
                        .Select(dataTableCellBlueprint => new DataTableCellBlueprintDto(dataTableCellBlueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        ;
        }
    }
}