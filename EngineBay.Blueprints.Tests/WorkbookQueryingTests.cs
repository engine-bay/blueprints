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

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task EmptyPaginationParametersBringsDataWorkbooks()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters();

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooks()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingPaginationParametersShouldBringBackNoWorkbooksButTheTotalShouldStillBeThere()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 0,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedWorkbooksCanBeControlled()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                Limit = 2,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSorted()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedInReverse()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 4 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortBy = "Name",
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }

        [Fact]
        public async Task PaginatedWorkbooksCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var paginationParameters = new PaginationParameters()
            {
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(paginationParameters, CancellationToken.None).ConfigureAwait(false);
            var first = dto.Data.First();
            Assert.Equal("K-Factor 1 test workbook", first.Name);
        }
    }
}
