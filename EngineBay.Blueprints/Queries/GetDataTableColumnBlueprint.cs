namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetDataTableColumnBlueprint : IQueryHandler<Guid, DataTableColumnBlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetDataTableColumnBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<DataTableColumnBlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.DataTableColumnBlueprints
                        .Where(x => x.Id == id)
                        .Select(dataTableColumnBlueprint => new DataTableColumnBlueprintDto(dataTableColumnBlueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        .ConfigureAwait(false);
        }
    }
}