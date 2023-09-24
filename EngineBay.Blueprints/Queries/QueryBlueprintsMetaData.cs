namespace EngineBay.Blueprints
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryBlueprintsMetaData : PaginatedQuery<Blueprint>, IQueryHandler<FilteredPaginationParameters<Blueprint>, PaginatedDto<BlueprintMetaDataDto>>
    {
        private readonly BlueprintsQueryDbContext db;

        public QueryBlueprintsMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<PaginatedDto<BlueprintMetaDataDto>> Handle(FilteredPaginationParameters<Blueprint> filteredPaginationParameters, CancellationToken cancellation)
        {
            if (filteredPaginationParameters is null)
            {
                throw new ArgumentNullException(nameof(filteredPaginationParameters));
            }

            var limit = filteredPaginationParameters.Limit;
            var skip = limit > 0 ? filteredPaginationParameters.Skip : 0;
            var filterPredicate = filteredPaginationParameters.FilterPredicate is null ? x => true : filteredPaginationParameters.FilterPredicate;

            var total = await this.db.Blueprints.Where(filterPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.Blueprints
                        .Where(filterPredicate)
                       .AsExpandable();

            Expression<Func<Blueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                nameof(Blueprint.CreatedAt) => blueprint => blueprint.CreatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.LastUpdatedAt) => blueprint => blueprint.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                nameof(Blueprint.Name) => blueprint => blueprint.Name,
                nameof(Blueprint.Description) => blueprint => blueprint.Description,
                _ => throw new ArgumentNullException(filteredPaginationParameters.SortBy),
            };

            query = this.Sort(query, sortByPredicate, filteredPaginationParameters);
            query = this.Paginate(query, filteredPaginationParameters);

            var blueprintDtos = limit > 0 ? await query
                 .Select(blueprint => new BlueprintMetaDataDto(blueprint))
                 .ToListAsync(cancellation)
                 .ConfigureAwait(false) : new List<BlueprintMetaDataDto>();

            return new PaginatedDto<BlueprintMetaDataDto>(total, skip, limit, blueprintDtos);
        }
    }
}