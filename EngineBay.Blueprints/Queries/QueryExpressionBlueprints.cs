namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryExpressionBlueprints : PaginatedQuery<ExpressionBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<ExpressionBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryExpressionBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<ExpressionBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.ExpressionBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.ExpressionBlueprints
                            .Include(x => x.InputDataTableBlueprints)
                            .Include(x => x.InputDataVariableBlueprints)
                            .Include(x => x.OutputDataVariableBlueprint)
                            .AsExpandable();

            Expression<Func<ExpressionBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(ExpressionBlueprint.CreatedAt) => expressionBlueprint => expressionBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(ExpressionBlueprint.LastUpdatedAt) => expressionBlueprint => expressionBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(ExpressionBlueprint.Expression) => expressionBlueprint => expressionBlueprint.Expression,
                nameof(ExpressionBlueprint.Objective) => expressionBlueprint => expressionBlueprint.Objective,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var expressionBlueprintDtos = limit > 0 ? await query
                .Select(expressionBlueprint => new ExpressionBlueprintDto(expressionBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<ExpressionBlueprintDto>();

            return new PaginatedDto<ExpressionBlueprintDto>(total, skip, limit, expressionBlueprintDtos);
        }
    }
}