namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class ExpressionBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/expression-blueprints", async (QueryExpressionBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints", async (QueryExpressionBlueprints query, Guid blueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(searchParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/expression-blueprints/{id}", async (GetExpressionBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapPost("/expression-blueprints", async (CreateExpressionBlueprint command, ExpressionBlueprint expressionBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(expressionBlueprint, cancellation);
                return Results.Created($"/expression-blueprints/{expressionBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapPut("/expression-blueprints/{id}", async (UpdateExpressionBlueprint command, ExpressionBlueprint updateExpressionBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<ExpressionBlueprint>
                {
                    Id = id,
                    Entity = updateExpressionBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapDelete("/expression-blueprints/{id}", async (DeleteExpressionBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });
        }
    }
}