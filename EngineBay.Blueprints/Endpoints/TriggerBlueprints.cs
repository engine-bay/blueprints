namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class TriggerBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/trigger-blueprints", async (QueryTriggerBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints", async (QueryTriggerBlueprints query, Guid blueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerBlueprint>(searchParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapGet("/trigger-blueprints/{id}", async (GetTriggerBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapPost("/trigger-blueprints", async (CreateTriggerBlueprint command, TriggerBlueprint triggerBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(triggerBlueprint, cancellation);
                return Results.Created($"/trigger-blueprints/{triggerBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapPut("/trigger-blueprints/{id}", async (UpdateTriggerBlueprint command, TriggerBlueprint updateTriggerBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<TriggerBlueprint>
                {
                    Id = id,
                    Entity = updateTriggerBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapDelete("/trigger-blueprints/{id}", async (DeleteTriggerBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });
        }
    }
}