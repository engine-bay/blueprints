namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class WorkbookQueryingTests : BaseBlueprintsCommandTest
    {
        public WorkbookQueryingTests()
            : base(nameof(WorkbookQueryingTests))
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
            };

            var path = Path.GetFullPath(@"./TestData/searchable-workbooks.json");
            List<Workbook>? workbooks = JsonConvert.DeserializeObject<List<Workbook>>(File.ReadAllText(path));
            var workbooksCount = this.BlueprintsDbContext.Workbooks.Count();
            if (workbooks is not null)
            {
                if (workbooksCount == 0)
                {
                    this.BlueprintsDbContext.AddRange(workbooks);

                    var applicationUser = new MockApplicationUser();

                    this.BlueprintsDbContext.SaveChanges(applicationUser);
                }
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfWorkbooks()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataWorkbooks()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooks()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooksButTheTotalShouldStillBeThere()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedWorkbooksCanBeControlled()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 2,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSorted()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedInReverse()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 4 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryWorkbooks(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }
    }
}
