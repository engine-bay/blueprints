namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryOutputDataVariableBlueprints : PaginatedQuery<OutputDataVariableBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<OutputDataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryOutputDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<OutputDataVariableBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.OutputDataVariableBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.OutputDataVariableBlueprints.AsExpandable();

            Expression<Func<OutputDataVariableBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(OutputDataVariableBlueprint.CreatedAt) => outputDataVariableBlueprint => outputDataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(OutputDataVariableBlueprint.LastUpdatedAt) => outputDataVariableBlueprint => outputDataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(OutputDataVariableBlueprint.Name) => outputDataVariableBlueprint => outputDataVariableBlueprint.Name,
                nameof(OutputDataVariableBlueprint.Namespace) => outputDataVariableBlueprint => outputDataVariableBlueprint.Namespace,
                nameof(OutputDataVariableBlueprint.Type) => outputDataVariableBlueprint => outputDataVariableBlueprint.Type,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var outputDataVariableBlueprintDtos = limit > 0 ? await query
                .Select(outputDataVariableBlueprint => new OutputDataVariableBlueprintDto(outputDataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<OutputDataVariableBlueprintDto>();

            return new PaginatedDto<OutputDataVariableBlueprintDto>(total, skip, limit, outputDataVariableBlueprintDtos);
        }
    }
}