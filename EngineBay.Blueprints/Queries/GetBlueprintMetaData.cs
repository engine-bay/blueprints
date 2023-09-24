namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetBlueprintMetaData : IQueryHandler<Guid, BlueprintMetaDataDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetBlueprintMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<BlueprintMetaDataDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.Blueprints
                        .Where(x => x.Id == id)
                        .Select(blueprint => new BlueprintDto(blueprint))
                        .AsExpandable()
                        .FirstAsync(cancellation)
                        .ConfigureAwait(false);
        }
    }
}