namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class BlueprintQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public BlueprintQueryingTests()
            : base(nameof(BlueprintQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-blueprints.json");
            List<Blueprint>? blueprints = JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText(path));

            if (blueprints is not null)
            {
                this.DbContext.AddRange(blueprints);

                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfBlueprints()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataBlueprints()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(8, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoBlueprints()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoBlueprintsButTheTotalShouldStillBeThere()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedBlueprintsCanBeControlled()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 2,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSorted()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedInReverse()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 7", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryBlueprints(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }
    }
}
