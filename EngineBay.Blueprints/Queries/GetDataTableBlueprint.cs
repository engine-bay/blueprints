namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetDataTableBlueprint : IQueryHandler<Guid, DataTableBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetDataTableBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.DataTableBlueprints
                        .Include(x => x.InputDataVariableBlueprints)
                        .Include(x => x.DataTableColumnBlueprints)
                        .Include(x => x.DataTableRowBlueprints)
                            .ThenInclude(x => x.DataTableCellBlueprints)
                        .Where(x => x.Id == id)
                        .Select(dataTableBlueprint => new DataTableBlueprintDto(dataTableBlueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        ;
        }
    }
}