namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsEngineWriteDb : BlueprintsEngineDb
    {
        public BlueprintsEngineWriteDb(DbContextOptions<EngineWriteDb> options)
            : base(options)
        {
        }
    }
}