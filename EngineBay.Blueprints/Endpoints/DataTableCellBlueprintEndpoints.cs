namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using Microsoft.AspNetCore.Mvc;

    public static class DataTableCellBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-cell-blueprints", async (QueryDataTableCellBlueprints query, [FromQuery] FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableCellBlueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-row-blueprints/{dataTableRowBlueprintId}/data-table-cell-blueprints", async (QueryDataTableCellBlueprints query, Guid dataTableRowBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableCellBlueprint>(paginationParameters, x => x.DataTableRowBlueprintId == dataTableRowBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapGet("/data-table-cell-blueprints/{id}", async (GetDataTableCellBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapPost("/data-table-cell-blueprints", async (CreateDataTableCellBlueprint command, DataTableCellBlueprint dataTableCellBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableCellBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-table-cell-blueprints/{dataTableCellBlueprint.Id}", dto);
            }).RequireAuthorization()
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
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });

            endpoints.MapDelete("/data-table-cell-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableCellBlueprints,
            });
        }
    }
}