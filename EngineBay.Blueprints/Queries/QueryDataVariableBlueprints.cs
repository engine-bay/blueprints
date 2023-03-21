namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataVariableBlueprints : IQueryHandler<PaginationParameters, PaginatedDto<DataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataVariableBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit.GetValueOrDefault();
            var skip = limit > 0 ? paginationParameters.Skip.GetValueOrDefault() : 0;

            var total = await this.db.DataVariableBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var dataVariableBlueprints = limit > 0 ? await this.db.DataVariableBlueprints.OrderByDescending(x => x.LastUpdatedAt).Skip(skip).Take(limit).ToListAsync(cancellation).ConfigureAwait(false) : new List<DataVariableBlueprint>();

            var dataVariableBlueprintDtos = dataVariableBlueprints.Select(dataVariableBlueprint => new DataVariableBlueprintDto(dataVariableBlueprint)).ToList();

            return new PaginatedDto<DataVariableBlueprintDto>(total, skip, limit, dataVariableBlueprintDtos);
        }
    }
}