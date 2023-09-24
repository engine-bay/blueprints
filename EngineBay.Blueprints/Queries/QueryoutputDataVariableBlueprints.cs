namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryOutputDataVariableBlueprints : PaginatedQuery<OutputDataVariableBlueprint>, IQueryHandler<FilteredPaginationParameters<OutputDataVariableBlueprint>, PaginatedDto<OutputDataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryOutputDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<OutputDataVariableBlueprintDto>> Handle(FilteredPaginationParameters<OutputDataVariableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var total = await this.db.OutputDataVariableBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.OutputDataVariableBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<OutputDataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(OutputDataVariableBlueprint.CreatedAt) => outputDataVariableBlueprint => outputDataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(OutputDataVariableBlueprint.LastUpdatedAt) => outputDataVariableBlueprint => outputDataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(OutputDataVariableBlueprint.Name) => outputDataVariableBlueprint => outputDataVariableBlueprint.Name,
                nameof(OutputDataVariableBlueprint.Namespace) => outputDataVariableBlueprint => outputDataVariableBlueprint.Namespace,
                nameof(OutputDataVariableBlueprint.Type) => outputDataVariableBlueprint => outputDataVariableBlueprint.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var outputDataVariableBlueprintDtos = limit > 0 ? await query
                .Select(outputDataVariableBlueprint => new OutputDataVariableBlueprintDto(outputDataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<OutputDataVariableBlueprintDto>();

            return new PaginatedDto<OutputDataVariableBlueprintDto>(total, skip, limit, outputDataVariableBlueprintDtos);
        }
    }
}