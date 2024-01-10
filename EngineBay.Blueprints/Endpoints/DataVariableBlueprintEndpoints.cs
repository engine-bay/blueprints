namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class DataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-variable-blueprints", async (QueryDataVariableBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-variable-blueprints", async (QueryDataVariableBlueprints query, Guid blueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>(searchParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapGet("/data-variable-blueprints/{id}", async (GetDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapPost("/data-variable-blueprints", async (CreateDataVariableBlueprint command, DataVariableBlueprint dataVariableBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataVariableBlueprint, cancellation);
                return Results.Created($"/data-variable-blueprints/{dataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapPut("/data-variable-blueprints/{id}", async (UpdateDataVariableBlueprint command, DataVariableBlueprint updateDataVariableBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapDelete("/data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });
        }
    }
}