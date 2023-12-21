namespace EngineBay.Blueprints
{
    using System;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryExpressionBlueprintsMetaData : PaginatedQuery<ExpressionBlueprint>, IQueryHandler<FilteredPaginationParameters<ExpressionBlueprint>, PaginatedDto<ExpressionBlueprintMetaDataDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryExpressionBlueprintsMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<ExpressionBlueprintMetaDataDto>> Handle(FilteredPaginationParameters<ExpressionBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(filteredPaginationParameters);

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var search = filteredPaginationParameters.Search;
            Expression<Func<ExpressionBlueprint, bool>>? searchPredicate = entity => entity.Objective != null && EF.Functions.Like(entity.Objective, $"%{search}%");

            var total = await this.db.ExpressionBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation);

            var query = this.db.ExpressionBlueprints
                            .Where(filterPredicate)
                            .Where(searchPredicate)
                            .AsExpandable();
#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<ExpressionBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(ExpressionBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(ExpressionBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(ExpressionBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(ExpressionBlueprint.Expression), StringComparison.OrdinalIgnoreCase) => entity => entity.Expression,
                string sortBy when sortBy.Equals(nameof(ExpressionBlueprint.Objective), StringComparison.OrdinalIgnoreCase) => entity => entity.Objective,
                _ => throw new ArgumentException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var expressionBlueprintDtos = limit > 0 ? await query
                .Select(expressionBlueprint => new ExpressionBlueprintMetaDataDto(expressionBlueprint))
                .ToListAsync(cancellation)
                 : new List<ExpressionBlueprintMetaDataDto>();

            return new PaginatedDto<ExpressionBlueprintMetaDataDto>(total, skip, limit, expressionBlueprintDtos);
        }
    }
}