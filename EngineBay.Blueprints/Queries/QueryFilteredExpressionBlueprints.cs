namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredExpressionBlueprints : PaginatedQuery<ExpressionBlueprint>, IQueryHandler<FilteredPaginationParameters<ExpressionBlueprint>, PaginatedDto<ExpressionBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredExpressionBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<ExpressionBlueprintDto>> Handle(FilteredPaginationParameters<ExpressionBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate;

            if (filterPredicate is null)
            {
                throw new ArgumentException(nameof(filterPredicate));
            }

            var total = await this.db.ExpressionBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.ExpressionBlueprints
                            .Include(x => x.InputDataTableBlueprints)
                            .Include(x => x.InputDataVariableBlueprints)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .Where(filterPredicate)
                            .AsExpandable();

            Expression<Func<ExpressionBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(ExpressionBlueprint.CreatedAt) => expressionBlueprint => expressionBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(ExpressionBlueprint.LastUpdatedAt) => expressionBlueprint => expressionBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(ExpressionBlueprint.Expression) => expressionBlueprint => expressionBlueprint.Expression,
                nameof(ExpressionBlueprint.Objective) => expressionBlueprint => expressionBlueprint.Objective,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var expressionBlueprintDtos = limit > 0 ? await query
                .Select(expressionBlueprint => new ExpressionBlueprintDto(expressionBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<ExpressionBlueprintDto>();

            return new PaginatedDto<ExpressionBlueprintDto>(total, skip, limit, expressionBlueprintDtos);
        }
    }
}