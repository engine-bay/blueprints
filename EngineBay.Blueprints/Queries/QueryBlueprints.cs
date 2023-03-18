namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class QueryBlueprints : IQueryHandler<PaginationParameters, PaginatedDto<BlueprintDto>>
    {
        private readonly BlueprintsEngineQueryDb db;

        public QueryBlueprints(BlueprintsEngineQueryDb db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<BlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit.GetValueOrDefault();
            var skip = limit > 0 ? paginationParameters.Skip.GetValueOrDefault() : 0;

            var total = await this.db.Blueprints.CountAsync(cancellation).ConfigureAwait(false);

            var blueprints = limit > 0 ? await this.db.Blueprints.OrderByDescending(x => x.LastUpdatedAt).Skip(skip).Take(limit).ToListAsync(cancellation).ConfigureAwait(false) : new List<Blueprint>();

            var blueprintDtos = blueprints.Select(blueprint => new BlueprintDto(blueprint)).ToList();

            return new PaginatedDto<BlueprintDto>(total, skip, limit, blueprintDtos);
        }
    }
}