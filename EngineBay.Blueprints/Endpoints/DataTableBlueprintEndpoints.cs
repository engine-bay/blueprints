namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using Microsoft.AspNetCore.Mvc;

    public static class DataTableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-blueprints", async (QueryDataTableBlueprints query, [FromQuery] FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableBlueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints", async (QueryDataTableBlueprints query, Guid blueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableBlueprint>(paginationParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapGet("/data-table-blueprints/{id}", async (GetDataTableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapPost("/data-table-blueprints", async (CreateDataTableBlueprint command, DataTableBlueprint dataTableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-table-blueprints/{dataTableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapPut("/data-table-blueprints/{id}", async (UpdateDataTableBlueprint command, DataTableBlueprint updateDataTableBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });

            endpoints.MapDelete("/data-table-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableBlueprints,
            });
        }
    }
}