namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataVariableBlueprints : PaginatedQuery<DataVariableBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<DataVariableBlueprintDto>>
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

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.DataVariableBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataVariableBlueprints.AsExpandable();

            Expression<Func<DataVariableBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(DataVariableBlueprint.CreatedAt) => dataVariableBlueprint => dataVariableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataVariableBlueprint.LastUpdatedAt) => dataVariableBlueprint => dataVariableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataVariableBlueprint.Name) => dataVariableBlueprint => dataVariableBlueprint.Name,
                nameof(DataVariableBlueprint.Namespace) => dataVariableBlueprint => dataVariableBlueprint.Namespace,
                nameof(DataVariableBlueprint.Type) => dataVariableBlueprint => dataVariableBlueprint.Type,
                nameof(DataVariableBlueprint.DefaultValue) => dataVariableBlueprint => dataVariableBlueprint.DefaultValue,
                nameof(DataVariableBlueprint.Description) => dataVariableBlueprint => dataVariableBlueprint.Description,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var dataVariableBlueprintDtos = limit > 0 ? await query
                .Select(dataVariableBlueprint => new DataVariableBlueprintDto(dataVariableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataVariableBlueprintDto>();

            return new PaginatedDto<DataVariableBlueprintDto>(total, skip, limit, dataVariableBlueprintDtos);
        }
    }
}