namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class DataTableColumnBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-table-column-blueprints", async (QueryDataTableColumnBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableColumnBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/data-table-column-blueprints", async (QueryDataTableColumnBlueprints query, Guid dataTableBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<DataTableColumnBlueprint>(searchParameters, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });

            endpoints.MapGet("/data-table-column-blueprints/{id}", async (GetDataTableColumnBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });

            endpoints.MapPost("/data-table-column-blueprints", async (CreateDataTableColumnBlueprint command, DataTableColumnBlueprint dataTableColumnBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataTableColumnBlueprint, cancellation);
                return Results.Created($"/data-table-column-blueprints/{dataTableColumnBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });

            endpoints.MapPut("/data-table-column-blueprints/{id}", async (UpdateDataTableColumnBlueprint command, DataTableColumnBlueprint updateDataTableColumnBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataTableColumnBlueprint>
                {
                    Id = id,
                    Entity = updateDataTableColumnBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });

            endpoints.MapDelete("/data-table-column-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataTableColumnBlueprints,
            });
        }
    }
}