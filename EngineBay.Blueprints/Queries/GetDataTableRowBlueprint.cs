namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetDataTableRowBlueprint : IQueryHandler<Guid, DataTableRowBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetDataTableRowBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableRowBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.DataTableRowBlueprints
                        .Include(x => x.DataTableCellBlueprints)
                        .Where(x => x.Id == id)
                        .Select(dataTableRowBlueprint => new DataTableRowBlueprintDto(dataTableRowBlueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        ;
        }
    }
}