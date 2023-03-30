namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class DataTableRowBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-row-blueprints", async (QueryDataTableRowBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-row-blueprints", async (QueryFilteredDataTableRowBlueprints query, Guid dataTableBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableRowBlueprint>(skip, limit, sortBy, sortOrder, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapGet("/data-table-row-blueprints/{id}", async (GetDataTableRowBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapPost("/data-table-row-blueprints", async (CreateDataTableRowBlueprint command, DataTableRowBlueprint dataTableRowBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableRowBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-table-row-blueprints/{dataTableRowBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapPut("/data-table-row-blueprints/{id}", async (UpdateDataTableRowBlueprint command, DataTableRowBlueprint updateDataTableRowBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableRowBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableRowBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });

            endpoints.MapDelete("/data-table-row-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataTableRowBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableRowBlueprints,
            });
        }
    }
}