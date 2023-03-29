namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryInputDataVariableBlueprints : PaginatedQuery<InputDataVariableBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<InputDataVariableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryInputDataVariableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<InputDataVariableBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.InputDataVariableBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.InputDataVariableBlueprints.AsExpandable();

            Expression<Func<InputDataVariableBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(InputDataVariableBlueprint.CreatedAt) => inputDataVariableBlueprint => inputDataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(InputDataVariableBlueprint.LastUpdatedAt) => inputDataVariableBlueprint => inputDataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(InputDataVariableBlueprint.Name) => inputDataVariableBlueprint => inputDataVariableBlueprint.Name,
                nameof(InputDataVariableBlueprint.Namespace) => inputDataVariableBlueprint => inputDataVariableBlueprint.Namespace,
                nameof(InputDataVariableBlueprint.Type) => inputDataVariableBlueprint => inputDataVariableBlueprint.Type,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var inputDataVariableBlueprintDtos = limit > 0 ? await query
                .Select(inputDataVariableBlueprint => new InputDataVariableBlueprintDto(inputDataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<InputDataVariableBlueprintDto>();

            return new PaginatedDto<InputDataVariableBlueprintDto>(total, skip, limit, inputDataVariableBlueprintDtos);
        }
    }
}