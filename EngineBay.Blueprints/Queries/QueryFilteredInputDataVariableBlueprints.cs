namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryFilteredInputDataVariableBlueprints : PaginatedQuery<InputDataVariableBlueprint>, IQueryHandler<FilteredPaginationParameters<InputDataVariableBlueprint>, PaginatedDto<InputDataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryFilteredInputDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<InputDataVariableBlueprintDto>> Handle(FilteredPaginationParameters<InputDataVariableBlueprint> filteredPaginationParameters, CancellationToken cancellation)
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

            var total = await this.db.InputDataVariableBlueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.InputDataVariableBlueprints.Where(filterPredicate).AsExpandable();

            Expression<Func<InputDataVariableBlueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(InputDataVariableBlueprint.CreatedAt) => inputDataVariableBlueprint => inputDataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(InputDataVariableBlueprint.LastUpdatedAt) => inputDataVariableBlueprint => inputDataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(InputDataVariableBlueprint.Name) => inputDataVariableBlueprint => inputDataVariableBlueprint.Name,
                nameof(InputDataVariableBlueprint.Namespace) => inputDataVariableBlueprint => inputDataVariableBlueprint.Namespace,
                nameof(InputDataVariableBlueprint.Type) => inputDataVariableBlueprint => inputDataVariableBlueprint.Type,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var inputDataVariableBlueprintDtos = limit > 0 ? await query
                .Select(inputDataVariableBlueprint => new InputDataVariableBlueprintDto(inputDataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<InputDataVariableBlueprintDto>();

            return new PaginatedDto<InputDataVariableBlueprintDto>(total, skip, limit, inputDataVariableBlueprintDtos);
        }
    }
}