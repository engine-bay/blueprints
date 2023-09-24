namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using Microsoft.AspNetCore.Mvc;

    public static class ExpressionBlueprints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/expression-blueprints", async (QueryExpressionBlueprints query, [FromQuery] FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints", async (QueryExpressionBlueprints query, Guid blueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<ExpressionBlueprint>(paginationParameters, x => x.BlueprintId == blueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapGet("/expression-blueprints/{id}", async (GetExpressionBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapPost("/expression-blueprints", async (CreateExpressionBlueprint command, ExpressionBlueprint expressionBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(expressionBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/expression-blueprints/{expressionBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapPut("/expression-blueprints/{id}", async (UpdateExpressionBlueprint command, ExpressionBlueprint updateExpressionBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<ExpressionBlueprint>
                {
                    Id = id,
                    Entity = updateExpressionBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });

            endpoints.MapDelete("/expression-blueprints/{id}", async (DeleteExpressionBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.ExpressionBlueprints,
            });
        }
    }
}