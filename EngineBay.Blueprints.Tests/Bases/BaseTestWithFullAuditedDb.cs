namespace EngineBay.Blueprints.Tests
{
    using System;
    using EngineBay.Auditing;
    using EngineBay.Authentication;
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BaseTestWithFullAuditedDb<TContext> : BaseTestWithDbContext<AuditingWriteDbContext>
        where TContext : ModuleDbContext
    {
        private bool isDisposed;

        public BaseTestWithFullAuditedDb(string databaseName)
            : base(databaseName + "AuditDb")
        {
            this.AuditDbContext = base.DbContext;

            var dbContextOptions = new DbContextOptionsBuilder<ModuleWriteDbContext>()
                    .UseInMemoryDatabase(databaseName)
                    .EnableSensitiveDataLogging()
                    .Options;

            var currentIdentity = new SystemUserIdentity();
            var interceptor = new AuditingInterceptor(currentIdentity, this.AuditDbContext);

            if (Activator.CreateInstance(typeof(TContext), dbContextOptions, interceptor) is not TContext context)
            {
                throw new ArgumentException("Context is null");
            }

            this.DbContext = context;
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Database.EnsureCreated();
        }

        protected new TContext DbContext { get; set; }

        protected AuditingWriteDbContext AuditDbContext { get; set; }

        protected void ResetAuditEntries()
        {
            this.AuditDbContext.AuditEntries.RemoveRange(this.AuditDbContext.AuditEntries);
            this.AuditDbContext.SaveChanges();
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                this.DbContext.Database.EnsureDeleted();
                this.DbContext.Dispose();

                base.Dispose(disposing);
            }

            this.isDisposed = true;
        }
    }
}
