namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class BlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/blueprints", async (QueryBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);

            endpoints.MapGet("/workbooks/{workbookId}/blueprints", async (QueryBlueprints query, Guid workbookId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(searchParameters, x => x.WorkbookId == workbookId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);

            endpoints.MapGet("/blueprints/{id}", async (GetBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);

            endpoints.MapPost("/blueprints", async (CreateBlueprint command, Blueprint blueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(blueprint, cancellation);
                return Results.Created($"/blueprints/{blueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);

            endpoints.MapPut("/blueprints/{id}", async (UpdateBlueprint command, Blueprint updateBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Blueprint>
                {
                    Id = id,
                    Entity = updateBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);

            endpoints.MapDelete("/blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(ApiGroupNameConstants.Blueprints);
        }
    }
}