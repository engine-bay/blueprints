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

            var search = filteredPaginationParameters.Search;
            Expression<Func<OutputDataVariableBlueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");

            var total = await this.db.OutputDataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.OutputDataVariableBlueprints.Where(filterPredicate).Where(searchPredicate).AsExpandable();

#pragma warning disable CA1305

            // DateTime Tostrings cannot CultureInfo.InvariantCulture because SQL does not know how to interpret this
            Expression<Func<OutputDataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(),
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.Namespace), StringComparison.OrdinalIgnoreCase) => entity => entity.Namespace,
                string sortBy when sortBy.Equals(nameof(OutputDataVariableBlueprint.Type), StringComparison.OrdinalIgnoreCase) => entity => entity.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };
#pragma warning restore CA1305
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