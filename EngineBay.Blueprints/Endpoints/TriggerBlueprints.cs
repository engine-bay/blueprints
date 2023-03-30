namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class TriggerBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/trigger-blueprints", async (QueryTriggerBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints", async (QueryFilteredTriggerBlueprints query, Guid blueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerBlueprint>(skip, limit, sortBy, sortOrder, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapGet("/trigger-blueprints/{id}", async (GetTriggerBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapPost("/trigger-blueprints", async (CreateTriggerBlueprint command, TriggerBlueprint triggerBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(triggerBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/trigger-blueprints/{triggerBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapPut("/trigger-blueprints/{id}", async (UpdateTriggerBlueprint command, TriggerBlueprint updateTriggerBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<TriggerBlueprint>
                {
                    Id = id,
                    Entity = updateTriggerBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });

            endpoints.MapDelete("/trigger-blueprints/{id}", async (DeleteTriggerBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerBlueprints,
            });
        }
    }
}