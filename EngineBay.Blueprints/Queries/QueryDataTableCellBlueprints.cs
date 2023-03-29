namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableCellBlueprints : PaginatedQuery<DataTableCellBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<DataTableCellBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableCellBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableCellBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.DataTableCellBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableCellBlueprints.AsExpandable();

            Expression<Func<DataTableCellBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(DataTableCellBlueprint.CreatedAt) => dataTableCellBlueprint => dataTableCellBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableCellBlueprint.LastUpdatedAt) => dataTableCellBlueprint => dataTableCellBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableCellBlueprint.Name) => dataTableCellBlueprint => dataTableCellBlueprint.Name,
                nameof(DataTableCellBlueprint.Namespace) => dataTableCellBlueprint => dataTableCellBlueprint.Namespace,
                nameof(DataTableCellBlueprint.Key) => dataTableCellBlueprint => dataTableCellBlueprint.Key,
                nameof(DataTableCellBlueprint.Value) => dataTableCellBlueprint => dataTableCellBlueprint.Value,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var dataTableCellBlueprintDtos = limit > 0 ? await query
                .Select(dataTableCellBlueprint => new DataTableCellBlueprintDto(dataTableCellBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableCellBlueprintDto>();

            return new PaginatedDto<DataTableCellBlueprintDto>(total, skip, limit, dataTableCellBlueprintDtos);
        }
    }
}