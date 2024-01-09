namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class ExpressionBlueprintsMetaDataEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/meta-data/expression-blueprints", async (QueryExpressionBlueprintsMetaData query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/meta-data/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints", async (QueryExpressionBlueprintsMetaData query, Guid blueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(searchParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/meta-data/expression-blueprints/{id}", async (GetExpressionBlueprintMetaData query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapDelete("/meta-data/expression-blueprints/{id}", async (DeleteExpressionBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.MetaData,
                ApiGroupNameConstants.ExpressionBlueprints,
            });
        }
    }
}