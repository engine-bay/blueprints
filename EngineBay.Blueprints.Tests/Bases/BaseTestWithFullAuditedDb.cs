namespace EngineBay.Blueprints.Tests
{
    using System;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;
    using NSubstitute;

    public class BaseTestWithFullAuditedDb<TContext> : IDisposable
        where TContext : ModuleDbContext
    {
        private bool isDisposed;

        public BaseTestWithFullAuditedDb(string databaseName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<ModuleWriteDbContext>()
                    .UseInMemoryDatabase(databaseName)
                    .EnableSensitiveDataLogging()
                    .Options;

            var currentIdentity = Substitute.For<ICurrentIdentity>();
            currentIdentity.UserId.Returns(Guid.NewGuid());
            currentIdentity.Username.Returns("bob");

            var auditingInterceptor = Substitute.For<IAuditingInterceptor>();
            var auditableModelInterceptor = new AuditableModelInterceptor(currentIdentity);

            if (Activator.CreateInstance(typeof(TContext), dbContextOptions, auditingInterceptor, auditableModelInterceptor) is not TContext context)
            {
                throw new ArgumentException("Context is null");
            }

            this.DbContext = context;
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Database.EnsureCreated();
        }

        protected TContext DbContext { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
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
            }

            this.isDisposed = true;
        }
    }
}
