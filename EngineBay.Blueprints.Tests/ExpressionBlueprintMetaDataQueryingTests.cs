namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using Newtonsoft.Json;
    using Xunit;

    public class ExpressionBlueprintMetaDataQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public ExpressionBlueprintMetaDataQueryingTests()
            : base(nameof(ExpressionBlueprintMetaDataQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-expression-blueprints.json");
            List<ExpressionBlueprint>? expressionBlueprints = JsonConvert.DeserializeObject<List<ExpressionBlueprint>>(File.ReadAllText(path));
            var expressionBlueprintsCount = this.DbContext.ExpressionBlueprints.Count();
            if (expressionBlueprints is not null)
            {
                if (expressionBlueprintsCount == 0)
                {
                    this.DbContext.AddRange();

                    this.DbContext.SaveChanges();
                }
            }
        }

        [Fact]
        public async Task EmptyParametersBringsBackAPagedSetOfData()
        {
            var query = new QueryExpressionBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task MetaDataCanBeSearched()
        {
            var query = new QueryExpressionBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Search = "sneaky snakes",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("An expression for blueprint 0 and sneaky snakes", first.Objective);
            Assert.Equal(1, dto.Total);
        }

        [Fact]
        public async Task ASingleMetaDataCanBeSearched()
        {
            var query = new QueryExpressionBlueprintsMetaData(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Search = "sneaky snakes",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();

            var entityQuery = new GetExpressionBlueprintMetaData(this.DbContext);

            var metaData = await entityQuery.Handle(first.Id, CancellationToken.None);

            Assert.Equal("An expression for blueprint 0 and sneaky snakes", metaData.Objective);
        }
    }
}
