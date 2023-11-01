namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class DataTableRowBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-row-blueprints", async (QueryDataTableRowBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableRowBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-row-blueprints", async (QueryDataTableRowBlueprints query, Guid dataTableBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableRowBlueprint>(searchParameters, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapGet("/data-table-row-blueprints/{id}", async (GetDataTableRowBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapPost("/data-table-row-blueprints", async (CreateDataTableRowBlueprint command, DataTableRowBlueprint dataTableRowBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableRowBlueprint, cancellation);
                return Results.Created($"/data-table-row-blueprints/{dataTableRowBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapPut("/data-table-row-blueprints/{id}", async (UpdateDataTableRowBlueprint command, DataTableRowBlueprint updateDataTableRowBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableRowBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableRowBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapDelete("/data-table-row-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });
        }
    }
}