namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class DataTableCellBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-cell-blueprints", async (QueryDataTableCellBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-row-blueprints/{dataTableRowBlueprintId}/data-table-cell-blueprints", async (QueryFilteredDataTableCellBlueprints query, Guid dataTableRowBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableCellBlueprint>(skip, limit, sortBy, sortOrder, x => x.DataTableRowBlueprintId == dataTableRowBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/data-table-cell-blueprints/{id}", async (GetDataTableCellBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapPost("/data-table-cell-blueprints", async (CreateDataTableCellBlueprint command, DataTableCellBlueprint dataTableCellBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableCellBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-table-cell-blueprints/{dataTableCellBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapPut("/data-table-cell-blueprints/{id}", async (UpdateDataTableCellBlueprint command, DataTableCellBlueprint updateDataTableCellBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableCellBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableCellBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapDelete("/data-table-cell-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableCellBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });
        }
    }
}