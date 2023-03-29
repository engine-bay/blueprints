namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryDataTableRowBlueprints : PaginatedQuery<DataTableRowBlueprint>, IQueryHandler<PaginationParameters, PaginatedDto<DataTableRowBlueprintDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryDataTableRowBlueprints(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<DataTableRowBlueprintDto>> Handle(PaginationParameters paginationParameters, CancellationToken cancellation)
        {
            if (paginationParameters is null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            var limit = paginationParameters.Limit;
            var skip = limit > 0 ? paginationParameters.Skip : 0;

            var total = await this.db.DataTableRowBlueprints.CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.DataTableRowBlueprints.Include(x => x.DataTableCellBlueprints).AsExpandable();

            Expression<Func<DataTableRowBlueprint, string?>> sortByPredicate = paginationParameters.SortBy switch
            {
                nameof(DataTableRowBlueprint.CreatedAt) => dataTableRowBlueprint => dataTableRowBlueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(DataTableRowBlueprint.LastUpdatedAt) => dataTableRowBlueprint => dataTableRowBlueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                _ => throw new ArgumentNullException(paginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, paginationParameters);
            query = this.Paginate(query, paginationParameters);

            var dataTableRowBlueprintDtos = limit > 0 ? await query
                .Select(dataTableRowBlueprint => new DataTableRowBlueprintDto(dataTableRowBlueprint))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<DataTableRowBlueprintDto>();

            return new PaginatedDto<DataTableRowBlueprintDto>(total, skip, limit, dataTableRowBlueprintDtos);
        }
    }
}