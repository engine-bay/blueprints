namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsEngineQueryDb : BlueprintsEngineDb
    {
        public BlueprintsEngineQueryDb(DbContextOptions<EngineWriteDb> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new InvalidOperationException($"Tried to save changes on a read only db context {nameof(BlueprintsEngineQueryDb)}");
        }
    }
}