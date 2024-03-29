namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class TriggerExpressionBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/trigger-expression-blueprints", async (QueryTriggerExpressionBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerExpressionBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/trigger-expression-blueprints", async (QueryTriggerExpressionBlueprints query, Guid triggerBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<TriggerExpressionBlueprint>(searchParameters, x => x.TriggerBlueprintId == triggerBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });

            endpoints.MapGet("/trigger-expression-blueprints/{id}", async (GetTriggerExpressionBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });

            endpoints.MapPost("/trigger-expression-blueprints", async (CreateTriggerExpressionBlueprint command, TriggerExpressionBlueprint triggerExpressionBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(triggerExpressionBlueprint, cancellation);
                return Results.Created($"/trigger-expression-blueprints/{triggerExpressionBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });

            endpoints.MapPut("/trigger-expression-blueprints/{id}", async (UpdateTriggerExpressionBlueprint command, TriggerExpressionBlueprint updateTriggerExpressionBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<TriggerExpressionBlueprint>
                {
                    Id = id,
                    Entity = updateTriggerExpressionBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });

            endpoints.MapDelete("/trigger-expression-blueprints/{id}", async (DeleteTriggerExpressionBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.TriggerExpressionBlueprints,
            });
        }
    }
}