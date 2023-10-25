namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class WorkbookQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public WorkbookQueryingTests()
            : base(nameof(WorkbookQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-workbooks.json");
            List<Workbook>? workbooks = JsonConvert.DeserializeObject<List<Workbook>>(File.ReadAllText(path));
            var workbooksCount = this.DbContext.Workbooks.Count();
            if (workbooks is not null && workbooksCount == 0)
            {
                this.DbContext.AddRange(workbooks);
                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsBackAPagedSetOfWorkbooks()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataWorkbooks()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(3, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooks()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooksButTheTotalShouldStillBeThere()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedWorkbooksCanBeControlled()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Limit = 2,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSorted()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedInReverse()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 4 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task WorkbooksCanBeSearched()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Search = "2 test",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("K-Factor 2 test workbook", first.Name);
            Assert.Equal(1, dto.Total);
        }
    }
}
