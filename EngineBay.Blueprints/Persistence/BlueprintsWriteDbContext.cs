namespace EngineBay.Blueprints
{
    using EngineBay.Auditing;
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsWriteDbContext : BlueprintsQueryDbContext
    {
        private readonly IAuditingInterceptor auditingInterceptor;

        public BlueprintsWriteDbContext(DbContextOptions<ModuleWriteDbContext> options, IAuditingInterceptor auditingInterceptor)
            : base(options)
        {
            this.auditingInterceptor = auditingInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            optionsBuilder.AddInterceptors(this.auditingInterceptor);

            base.OnConfiguring(optionsBuilder);
        }
    }
}