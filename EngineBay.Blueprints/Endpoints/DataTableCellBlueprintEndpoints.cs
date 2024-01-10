namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class DataTableCellBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-cell-blueprints", async (QueryDataTableCellBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableCellBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-row-blueprints/{dataTableRowBlueprintId}/data-table-cell-blueprints", async (QueryDataTableCellBlueprints query, Guid dataTableRowBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableCellBlueprint>(searchParameters, x => x.DataTableRowBlueprintId == dataTableRowBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/data-table-cell-blueprints/{id}", async (GetDataTableCellBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapPost("/data-table-cell-blueprints", async (CreateDataTableCellBlueprint command, DataTableCellBlueprint dataTableCellBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableCellBlueprint, cancellation);
                return Results.Created($"/data-table-cell-blueprints/{dataTableCellBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapPut("/data-table-cell-blueprints/{id}", async (UpdateDataTableCellBlueprint command, DataTableCellBlueprint updateDataTableCellBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableCellBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableCellBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapDelete("/data-table-cell-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });
        }
    }
}