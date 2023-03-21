namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsQueryDbContext : BlueprintsDbContext
    {
        public BlueprintsQueryDbContext(DbContextOptions<ModuleWriteDbContext> options)
            : base(options)
        {
        }
    }
}