namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableColumnBlueprints : PaginatedQuery<DataTableColumnBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<DataTableColumnBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableColumnBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableColumnBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.DataTableColumnBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableColumnBlueprints.AsExpandable();

            Expression<Func<DataTableColumnBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(DataTableColumnBlueprint.CreatedAt) => dataTableColumnBlueprint => dataTableColumnBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableColumnBlueprint.LastUpdatedAt) => dataTableColumnBlueprint => dataTableColumnBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableColumnBlueprint.Name) => dataTableColumnBlueprint => dataTableColumnBlueprint.Name,
                nameof(DataTableColumnBlueprint.Type) => dataTableColumnBlueprint => dataTableColumnBlueprint.Type,
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var dataTableColumnBlueprintDtos = limit > 0 ? await query
                .Select(dataTableColumnBlueprint => new DataTableColumnBlueprintDto(dataTableColumnBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableColumnBlueprintDto>();

            return new PaginatedDto<DataTableColumnBlueprintDto>(total, skip, limit, dataTableColumnBlueprintDtos);
        }
    }
}