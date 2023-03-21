namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class QueryExpressionBlueprints : IQueryHandler<PaginationParameters, PaginatedDto<ExpressionBlueprintDto>>
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

            var limit = paginationParameters.Limit.GetValueOrDefault();
            var skip = limit > 0 ? paginationParameters.Skip.GetValueOrDefault() : 0;

            var total = await this.db.ExpressionBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var expressionBlueprints = limit > 0 ? await this.db.ExpressionBlueprints.OrderByDescending(x => x.LastUpdatedAt).Skip(skip).Take(limit).ToListAsync(cancellation).ConfigureAwait(false) : new List<ExpressionBlueprint>();

            var expressionBlueprintDtos = expressionBlueprints.Select(expressionBlueprint => new ExpressionBlueprintDto(expressionBlueprint)).ToList();

            return new PaginatedDto<ExpressionBlueprintDto>(total, skip, limit, expressionBlueprintDtos);
        }
    }
}