namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class DataVariableBlueprintQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public DataVariableBlueprintQueryingTests()
            : base(nameof(DataVariableBlueprintQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-data-variable-blueprints.json");
            List<DataVariableBlueprint>? dataVariableBlueprints = JsonConvert.DeserializeObject<List<DataVariableBlueprint>>(File.ReadAllText(path));
            var dataVariableBlueprintsCount = this.DbContext.DataVariableBlueprints.Count();
            if (dataVariableBlueprints is not null && dataVariableBlueprintsCount == 0)
            {
                this.DbContext.AddRange(dataVariableBlueprints);

                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfDataVariableBlueprints()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(24, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataDataVariableBlueprintsLimitedByThePageSize()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(10, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoDataVariableBlueprints()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoDataVariableBlueprintsButTheTotalShouldStillBeThere()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(24, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedDataVariableBlueprintsCanBeControlled()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                Limit = 2,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSorted()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("AllNumbersComparedTruthy0", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedInReverse()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("youngest_ivory_shrimp", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("musical_indigo_mink", first.Name);
        }

        [Fact]
        public async Task PaginatedDataVariableBlueprintsCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("musical_indigo_mink", first.Name);
        }

        [Fact]
        public async Task WorkbooksCanBeSearched()
        {
            var query = new QueryDataVariableBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>()
            {
                Search = "anaconda",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("eager_lavender_anaconda", first.Name);
            Assert.Equal(1, dto.Total);
        }
    }
}
