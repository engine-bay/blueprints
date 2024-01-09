namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Persistence;
    using Newtonsoft.Json;
    using Xunit;

    public class BlueprintMetaDataQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public BlueprintMetaDataQueryingTests()
            : base(nameof(BlueprintMetaDataQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-blueprints.json");
            List<Blueprint>? blueprints = JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText(path));
            var blueprintsCount = this.DbContext.Blueprints.Count();
            if (blueprints is not null && blueprintsCount == 0)
            {
                this.DbContext.AddRange(blueprints);
                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task EmptyParametersBringsBackAPagedSetOfData()
        {
            var query = new QueryBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task MetaDataCanBeSearched()
        {
            var query = new QueryBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Search = "1",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("Blueprint 1", first.Name);
            Assert.Equal(1, dto.Total);
        }

        [Fact]
        public async Task ASingleMetaDataCanBeSearched()
        {
            var query = new QueryBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Search = "1",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();

            var entityQuery = new GetBlueprintMetaData(this.DbContext);

            var metaData = await entityQuery.Handle(first.Id, CancellationToken.None);

            Assert.Equal("Blueprint 1", metaData.Name);
        }
    }
}
