namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class DataVariableBlueprintQueryingTests : BaseBlueprintsCommandTest
    {
        public DataVariableBlueprintQueryingTests()
            : base(nameof(DataVariableBlueprintQueryingTests))
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };

            var path = Path.GetFullPath(@"./TestData/searchable-data-variable-blueprints.json");
            List<DataVariableBlueprint>? dataVariableBlueprints = JsonConvert.DeserializeObject<List<DataVariableBlueprint>>(File.ReadAllText(path));
            var dataVariableBlueprintsCount = this.BlueprintsDbContext.DataVariableBlueprints.Count();
            if (dataVariableBlueprints is not null)
            {
                if (dataVariableBlueprintsCount == 0)
                {
                    this.BlueprintsDbContext.AddRange(dataVariableBlueprints);

                    var applicationUser = new MockApplicationUser();

                    this.BlueprintsDbContext.SaveChanges(applicationUser);
                }
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfDataVariableBlueprints()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(24, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataDataVariableBlueprintsLimitedByThePageSize()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(10, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoDataVariableBlueprints()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoDataVariableBlueprintsButTheTotalShouldStillBeThere()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(24, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedDataVariableBlueprintsCanBeControlled()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 2,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSorted()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("AllNumbersComparedTruthy0", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedInReverse()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("youngest_ivory_shrimp", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("musical_indigo_mink", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryDataVariableBlueprints(this.BlueprintsDbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("musical_indigo_mink", first.Name);
        }
    }
}
