namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class DataTableColumnBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-column-blueprints", async (QueryDataTableColumnBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-column-blueprints", async (QueryFilteredDataTableColumnBlueprints query, Guid dataTableBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableColumnBlueprint>(skip, limit, sortBy, sortOrder, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);

            endpoints.MapGet("/data-table-column-blueprints/{id}", async (GetDataTableColumnBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);

            endpoints.MapPost("/data-table-column-blueprints", async (CreateDataTableColumnBlueprint command, DataTableColumnBlueprint dataTableColumnBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableColumnBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-table-column-blueprints/{dataTableColumnBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);

            endpoints.MapPut("/data-table-column-blueprints/{id}", async (UpdateDataTableColumnBlueprint command, DataTableColumnBlueprint updateDataTableColumnBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableColumnBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableColumnBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);

            endpoints.MapDelete("/data-table-column-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableColumnBlueprints);
        }
    }
}