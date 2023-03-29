namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableBlueprints : PaginatedQuery<DataTableBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<DataTableBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.DataTableBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableBlueprints.AsExpandable();

            Expression<Func<DataTableBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(DataTableBlueprint.CreatedAt) => dataTableBlueprint => dataTableBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableBlueprint.LastUpdatedAt) => dataTableBlueprint => dataTableBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableBlueprint.Name) => dataTableBlueprint => dataTableBlueprint.Name,
                nameof(DataTableBlueprint.Namespace) => dataTableBlueprint => dataTableBlueprint.Namespace,
                nameof(DataTableBlueprint.Description) => dataTableBlueprint => dataTableBlueprint.Description,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var dataTableBlueprintDtos = limit > 0 ? await query
                .Select(dataTableBlueprint => new DataTableBlueprintDto(dataTableBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableBlueprintDto>();

            return new PaginatedDto<DataTableBlueprintDto>(total, skip, limit, dataTableBlueprintDtos);
        }
    }
}