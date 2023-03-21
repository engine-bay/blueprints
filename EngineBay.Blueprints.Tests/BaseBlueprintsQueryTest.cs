namespace EngineBay.Blueprints.Tests
{
    using EngineBay.Blueprints;
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public abstract class BaseBlueprintsQueryTest : IDisposable
    {
        private bool isDisposed;

        protected BaseBlueprintsQueryTest()
        {
            var mockProvider = new Mock<IServiceProvider>();

            var blueprintsDbContextOptions = new DbContextOptionsBuilder<ModuleWriteDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .EnableSensitiveDataLogging()
                .Options;

            this.BlueprintsDbContext = new BlueprintsQueryDbContext(blueprintsDbContextOptions);

            this.BlueprintsDbContext.Database.EnsureCreated();
        }

        protected BlueprintsQueryDbContext BlueprintsDbContext { get; set; }

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                this.BlueprintsDbContext.Database.EnsureDeleted();
                this.BlueprintsDbContext.Dispose();
            }

            this.isDisposed = true;
        }
    }
}
