namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsWriteDbContext : BlueprintsQueryDbContext
    {
        private readonly IAuditingInterceptor auditingInterceptor;
        private readonly AuditableModelInterceptor auditableModelInterceptor;

        public BlueprintsWriteDbContext(
            DbContextOptions<ModuleWriteDbContext> options,
            IAuditingInterceptor auditingInterceptor,
            AuditableModelInterceptor auditableModelInterceptor)
            : base(options)
        {
            this.auditingInterceptor = auditingInterceptor;
            this.auditableModelInterceptor = auditableModelInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);

            optionsBuilder.AddInterceptors(this.auditableModelInterceptor);
            optionsBuilder.AddInterceptors(this.auditingInterceptor);

            base.OnConfiguring(optionsBuilder);
        }
    }
}