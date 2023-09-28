namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class BlueprintQueryingTests : BaseBlueprintsCommandTest
    {
        public BlueprintQueryingTests()
            : base(nameof(BlueprintQueryingTests))
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };

            var path = Path.GetFullPath(@"./TestData/searchable-blueprints.json");
            List<Blueprint>? blueprints = JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText(path));
            var blueprintsCount = this.BlueprintsDbContext.Blueprints.Count();
            if (blueprints is not null)
            {
                if (blueprintsCount == 0)
                {
                    this.BlueprintsDbContext.AddRange(blueprints);

                    var applicationUser = new MockApplicationUser();

                    this.BlueprintsDbContext.SaveChanges(applicationUser);
                }
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfBlueprints()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataBlueprints()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoBlueprints()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoBlueprintsButTheTotalShouldStillBeThere()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedBlueprintsCanBeControlled()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Limit = 2,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSorted()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedInReverse()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 7", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }

        [Fact]
        public async Task PaginatedBlueprintsCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("Blueprint 0", first.Name);
        }

        [Fact]
        public async Task BlueprintsCanBeSearched()
        {
            var query = new QueryBlueprints(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Search = "1",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("Blueprint 1", first.Name);
            Assert.Equal(1, dto.Total);
        }
    }
}
