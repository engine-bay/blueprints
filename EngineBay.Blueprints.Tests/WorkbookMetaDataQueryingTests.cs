namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class WorkbookMetaDataQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public WorkbookMetaDataQueryingTests()
            : base(nameof(WorkbookMetaDataQueryingTests))
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
        public async Task EmptyParametersBringsBackAPagedSetOfData()
        {
            var query = new QueryWorkbooksMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(3, dto.Total);
        }

        [Fact]
        public async Task MetaDataCanBeSearched()
        {
            var query = new QueryWorkbooksMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Search = "2 test",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("K-Factor 2 test workbook", first.Name);
            Assert.Equal(1, dto.Total);
        }

        [Fact]
        public async Task ASingleMetaDataCanBeSearched()
        {
            var query = new QueryWorkbooks(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>()
            {
                Search = "2 test",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();

            var entityQuery = new GetWorkbookMetaData(this.DbContext);

            var metaData = await entityQuery.Handle(first.Id, CancellationToken.None);
            Assert.Equal("K-Factor 2 test workbook", metaData.Name);
        }
    }
}
