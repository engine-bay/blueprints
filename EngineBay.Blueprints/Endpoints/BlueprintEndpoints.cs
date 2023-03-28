namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class BlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            // Direct CRUD
            endpoints.MapGet("/blueprints", async (QueryBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
                        {
                            var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                            var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                            return Results.Ok(paginatedDtos);
                        }).RequireAuthorization();

            endpoints.MapGet("/blueprints/{id}", async (GetBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization();

            endpoints.MapPost("/blueprints", async (CreateBlueprint command, Blueprint blueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(blueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/blueprints/{blueprint.Id}", dto);
            }).RequireAuthorization();

            endpoints.MapPut("/blueprints/{id}", async (UpdateBlueprint command, Blueprint updateBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Blueprint>
                {
                    Id = id,
                    Entity = updateBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization();

            endpoints.MapDelete("/blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization();

            // Filtered Queries
            endpoints.MapGet("/workbooks/{workbookId}/blueprints", async (QueryBlueprints query, Guid workbookId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(skip, limit, sortBy, sortOrder, x => x.WorkbookId == workbookId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization();
        }
    }
}