namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class BlueprintMetaDataQueryingTests : BaseBlueprintsCommandTest
    {
        public BlueprintMetaDataQueryingTests()
            : base(nameof(BlueprintMetaDataQueryingTests))
        {
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
        public async Task EmptyParametersBringsBackAPagedSetOfData()
        {
            var query = new QueryBlueprintsMetaData(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task MetaDataCanBeSearched()
        {
            var query = new QueryBlueprintsMetaData(this.BlueprintsDbContext);

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
            var query = new QueryBlueprintsMetaData(this.BlueprintsDbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>()
            {
                Search = "1",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();

            var entityQuery = new GetBlueprintMetaData(this.BlueprintsDbContext);

            var metaData = await entityQuery.Handle(first.Id, CancellationToken.None);

            Assert.Equal("Blueprint 1", metaData.Name);
        }
    }
}