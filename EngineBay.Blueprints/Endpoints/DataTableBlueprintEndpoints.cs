namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class DataTableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-blueprints", async (QueryDataTableBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints", async (QueryDataTableBlueprints query, Guid blueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableBlueprint>(searchParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapGet("/data-table-blueprints/{id}", async (GetDataTableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapPost("/data-table-blueprints", async (CreateDataTableBlueprint command, DataTableBlueprint dataTableBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableBlueprint, cancellation);
                return Results.Created($"/data-table-blueprints/{dataTableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapPut("/data-table-blueprints/{id}", async (UpdateDataTableBlueprint command, DataTableBlueprint updateDataTableBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapDelete("/data-table-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });
        }
    }
}