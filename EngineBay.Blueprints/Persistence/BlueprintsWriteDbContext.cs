namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsWriteDbContext : BlueprintsQueryDbContext
    {
        public BlueprintsWriteDbContext(DbContextOptions<ModuleWriteDbContext> options)
            : base(options)
        {
        }
    }
}