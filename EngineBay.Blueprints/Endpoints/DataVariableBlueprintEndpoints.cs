namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class DataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/data-variable-blueprints", async (QueryDataVariableBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-variable-blueprints", async (QueryFilteredDataVariableBlueprints query, Guid blueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<DataVariableBlueprint>(skip, limit, sortBy, sortOrder, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapGet("/data-variable-blueprints/{id}", async (GetDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapPost("/data-variable-blueprints", async (CreateDataVariableBlueprint command, DataVariableBlueprint dataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataVariableBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-variable-blueprints/{dataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapPut("/data-variable-blueprints/{id}", async (UpdateDataVariableBlueprint command, DataVariableBlueprint updateDataVariableBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });

            endpoints.MapDelete("/data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.DataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.DataVariableBlueprints,
            });
        }
    }
}