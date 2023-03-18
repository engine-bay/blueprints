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

            var blueprintsDbOptions = new DbContextOptionsBuilder<EngineWriteDb>()
                .UseInMemoryDatabase(databaseName: "EngineDb")
                .EnableSensitiveDataLogging()
                .Options;

            this.BlueprintsDb = new BlueprintsEngineQueryDb(blueprintsDbOptions);

            this.BlueprintsDb.Database.EnsureCreated();
        }

        protected BlueprintsEngineQueryDb BlueprintsDb { get; set; }

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
                this.BlueprintsDb.Database.EnsureDeleted();
                this.BlueprintsDb.Dispose();
            }

            this.isDisposed = true;
        }
    }
}
