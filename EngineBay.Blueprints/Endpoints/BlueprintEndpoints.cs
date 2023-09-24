namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using Microsoft.AspNetCore.Mvc;

    public static class BlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/blueprints", async (QueryBlueprints query, [FromQuery] FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints", async (QueryBlueprints query, Guid workbookId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<Blueprint>(paginationParameters, x => x.WorkbookId == workbookId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapGet("/blueprints/{id}", async (GetBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapPost("/blueprints", async (CreateBlueprint command, Blueprint blueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(blueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/blueprints/{blueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapPut("/blueprints/{id}", async (UpdateBlueprint command, Blueprint updateBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Blueprint>
                {
                    Id = id,
                    Entity = updateBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });

            endpoints.MapDelete("/blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Blueprints,
            });
        }
    }
}