namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class WorkbookMetaDataEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/meta-data/workbooks", async (QueryWorkbooksMetaData query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>(searchParameters, filter);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapGet("/meta-data/workbooks/{id}", async (GetWorkbookMetaData query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.Workbooks,
            });
        }
    }
}