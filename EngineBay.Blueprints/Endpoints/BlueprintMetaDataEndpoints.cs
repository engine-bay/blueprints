namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class BlueprintMetaDataEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/meta-data/blueprints", async (QueryBlueprintsMetaData query, FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapGet("/meta-data/workbooks/{workbookId}/blueprints", async (QueryBlueprintsMetaData query, Guid workbookId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(paginationParameters, x => x.WorkbookId == workbookId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapGet("/meta-data/blueprints/{id}", async (GetBlueprintMetaData query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.Blueprints,
            });
        }
    }
}