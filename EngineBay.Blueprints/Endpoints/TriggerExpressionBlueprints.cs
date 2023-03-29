namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class TriggerExpressionBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/trigger-expression-blueprints", async (QueryTriggerExpressionBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/trigger-expression-blueprints", async (QueryFilteredTriggerExpressionBlueprints query, Guid triggerBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerExpressionBlueprint>(skip, limit, sortBy, sortOrder, x => x.TriggerBlueprintId == triggerBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);

            endpoints.MapGet("/trigger-expression-blueprints/{id}", async (GetTriggerExpressionBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);

            endpoints.MapPost("/trigger-expression-blueprints", async (CreateTriggerExpressionBlueprint command, TriggerExpressionBlueprint triggerExpressionBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(triggerExpressionBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/trigger-expression-blueprints/{triggerExpressionBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);

            endpoints.MapPut("/trigger-expression-blueprints/{id}", async (UpdateTriggerExpressionBlueprint command, TriggerExpressionBlueprint updateTriggerExpressionBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<TriggerExpressionBlueprint>
                {
                    Id = id,
                    Entity = updateTriggerExpressionBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);

            endpoints.MapDelete("/trigger-expression-blueprints/{id}", async (DeleteTriggerExpressionBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.TriggerExpressionBlueprints);
        }
    }
}