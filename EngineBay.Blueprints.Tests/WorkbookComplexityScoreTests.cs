namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using Newtonsoft.Json;
    using Xunit;

    public class WorkbookComplexityScoreTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public WorkbookComplexityScoreTests()
            : base(nameof(WorkbookComplexityScoreTests))
        {
            var path = Path.GetFullPath(@"./TestData/k-factor-workbooks.json");
            List<Workbook>? workbooks = JsonConvert.DeserializeObject<List<Workbook>>(File.ReadAllText(path));

            if (workbooks is not null)
            {
                this.DbContext.AddRange(workbooks);

                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task CanCalculateTheComplexityOfAFactor1Workbook()
        {
            var workbookId = new Guid("30be156a-91fd-4ae9-9dad-05e7ed44623a");

            var command = new GetWorkbookComplexityScore(this.DbContext, new GetWorkbook(this.DbContext));

            var dto = await command.Handle(workbookId, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2.6666666666666665, dto.Score);
        }

        [Fact]
        public async Task CanCalculateTheComplexityOfAFactor2Workbook()
        {
            var workbookId = new Guid("fdc26f9d-121e-40e5-b457-9a7ae6e264ad");

            var command = new GetWorkbookComplexityScore(this.DbContext, new GetWorkbook(this.DbContext));

            var dto = await command.Handle(workbookId, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(22, dto.Score);
        }

        [Fact]
        public async Task CanCalculateTheComplexityOfAFactor4Workbook()
        {
            var workbookId = new Guid("2802a321-6709-4a35-8e3d-cfd6fa6bfaf7");

            var command = new GetWorkbookComplexityScore(this.DbContext, new GetWorkbook(this.DbContext));

            var dto = await command.Handle(workbookId, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(228.33333333333334, dto.Score);
        }

        [Fact]
        public async Task CanCalculateTheComplexityOfAFactor8Workbook()
        {
            var workbookId = new Guid("e0dc623c-480c-4cf2-8c4e-e2f5d136ba3f");

            var command = new GetWorkbookComplexityScore(this.DbContext, new GetWorkbook(this.DbContext));

            var dto = await command.Handle(workbookId, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2715, dto.Score);
        }
    }
}
