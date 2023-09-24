namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryWorkbooks : PaginatedQuery<Workbook>, IQueryHandler<FilteredPaginationParameters<Workbook>, PaginatedDto<WorkbookDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryWorkbooks(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<WorkbookDto>> Handle(FilteredPaginationParameters<Workbook> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var total = await this.db.Workbooks.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.Workbooks
                .Include(x => x.Blueprints)
                        .ThenInclude(blueprint => blueprint.ExpressionBlueprints)
                            .ThenInclude(expressionBlueprint => expressionBlueprint.InputDataTableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.InputDataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.DataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.TriggerExpressionBlueprints)
                            .ThenInclude(x => x.InputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.InputDataVariableBlueprints)
                .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.DataTableColumnBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.DataTableBlueprints)
                        .ThenInclude(x => x.DataTableRowBlueprints)
                            .ThenInclude(x => x.DataTableCellBlueprints)
                .Where(filterPredicate)
                .AsExpandable();

            Expression<Func<Workbook, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(Workbook.CreatedAt) => workbook => workbook.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Workbook.LastUpdatedAt) => workbook => workbook.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Workbook.Name) => workbook => workbook.Name,
                nameof(Workbook.Description) => workbook => workbook.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var workbookDtos = limit > 0 ? await query
                .Select(workbook => new WorkbookDto(workbook))
                .ToListAsync(cancellation)
                .ConfigureAwait(false) : new List<WorkbookDto>();

            return new PaginatedDto<WorkbookDto>(total, skip, limit, workbookDtos);
        }
    }
}