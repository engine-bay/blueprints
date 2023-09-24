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
            var search = filteredPaginationParameters.Search;
            Expression<Func<Blueprint, bool>>? searchPredicate = entity => entity.Name != null && EF.Functions.Like(entity.Name, $"%{search}%");
            var total = await this.db.Blueprints.Where(filterPredicate).Where(searchPredicate).CountAsync(cancellation).ConfigureAwait(false);

            var query = this.db.Blueprints
                    .Where(filterPredicate)
                    .Where(searchPredicate)
                    .AsExpandable();

            Expression<Func<Blueprint, string?>> sortByPredicate = filteredPaginationParameters.SortBy switch
            {
                string sortBy when sortBy.Equals(nameof(Blueprint.Id), StringComparison.OrdinalIgnoreCase) => entity => entity.Id.ToString(),
                string sortBy when sortBy.Equals(nameof(Blueprint.CreatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.CreatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(Blueprint.LastUpdatedAt), StringComparison.OrdinalIgnoreCase) => entity => entity.LastUpdatedAt.ToString(CultureInfo.InvariantCulture),
                string sortBy when sortBy.Equals(nameof(Blueprint.Name), StringComparison.OrdinalIgnoreCase) => entity => entity.Name,
                string sortBy when sortBy.Equals(nameof(Blueprint.Description), StringComparison.OrdinalIgnoreCase) => entity => entity.Description,
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