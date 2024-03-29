namespace EngineBay.Blueprints.Tests
{
    using System.Threading.Tasks;
    using EngineBay.Blueprints;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using Newtonsoft.Json;
    using Xunit;

    public class ExpressionBlueprintQueryingTests : BaseTestWithFullAuditedDb<BlueprintsWriteDbContext>
    {
        public ExpressionBlueprintQueryingTests()
            : base(nameof(ExpressionBlueprintQueryingTests))
        {
            var path = Path.GetFullPath(@"./TestData/searchable-expression-blueprints.json");
            List<ExpressionBlueprint>? expressionBlueprints = JsonConvert.DeserializeObject<List<ExpressionBlueprint>>(File.ReadAllText(path));
            var expressionBlueprintsCount = this.DbContext.ExpressionBlueprints.Count();
            if (expressionBlueprints is not null && expressionBlueprintsCount == 0)
            {
                this.DbContext.AddRange(expressionBlueprints);
                this.DbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task EmptyfilteredPaginationParametersBringsBackAPagedSetOfExpressionBlueprints()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task EmptyfilteredPaginationParametersBringsDataExpressionBlueprints()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>();

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Data.Count());
        }

        [Fact]
        public async Task LimitingfilteredPaginationParametersShouldBringBackNoExpressionBlueprints()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Empty(dto.Data);
        }

        [Fact]
        public async Task LimitingfilteredPaginationParametersShouldBringBackNoExpressionBlueprintsButTheTotalShouldStillBeThere()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Limit = 0,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(8, dto.Total);
        }

        [Fact]
        public async Task ThePageSizeOfPaginatedExpressionBlueprintsCanBeControlled()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Limit = 2,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            Assert.Equal(2, dto.Data.Count());
        }

        [Fact]
        public async Task PaginatedExpressionBlueprintsCanBeSorted()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                SortBy = "Expression",
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("apparent_maroon_jackal", first.Expression);
        }

        [Fact]
        public async Task PaginatedExpressionBlueprintsCanBeSortedInReverse()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                SortBy = "Expression",
                SortOrder = SortOrderType.Descending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("youngest_ivory_shrimp + musical_indigo_mink + enthusiastic_maroon_lark * enthusiastic_maroon_lark + musical_indigo_mink / ill_aquamarine_panther - cognitive_turquoise_coyote - youngest_ivory_shrimp", first.Expression);
        }

        [Fact]
        public async Task PaginatedExpressionBlueprintsCanBeSortedButWithNoSpecifiedOrder()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                SortBy = "Expression",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("cognitive_turquoise_coyote - youngest_ivory_shrimp * enthusiastic_maroon_lark / musical_indigo_mink / ill_aquamarine_panther - youngest_ivory_shrimp - enthusiastic_maroon_lark - musical_indigo_mink", first.Expression);
        }

        [Fact]
        public async Task PaginatedExpressionBlueprintsCanBeSortedButWithNoSpecifiedOrderingProperty()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                SortOrder = SortOrderType.Ascending,
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);
            var first = dto.Data.First();
            Assert.Equal("cognitive_turquoise_coyote - youngest_ivory_shrimp * enthusiastic_maroon_lark / musical_indigo_mink / ill_aquamarine_panther - youngest_ivory_shrimp - enthusiastic_maroon_lark - musical_indigo_mink", first.Expression);
        }

        [Fact]
        public async Task ExpressionBlueprintsCanBeSearched()
        {
            var query = new QueryExpressionBlueprints(this.DbContext);

            var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>()
            {
                Search = "sneaky snakes",
            };

            var dto = await query.Handle(filteredPaginationParameters, CancellationToken.None);

            var first = dto.Data.First();
            Assert.Equal("An expression for blueprint 0 and sneaky snakes", first.Objective);
            Assert.Equal(1, dto.Total);
        }
    }
}
