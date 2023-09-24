namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using Microsoft.AspNetCore.Mvc;

    public static class OutputDataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, [FromQuery] FilterParameters? filter, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(paginationParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints/{expressionBlueprintId}/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, Guid expressionBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(paginationParameters, x => x.ExpressionBlueprintId == expressionBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, Guid triggerBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(paginationParameters, x => x.TriggerBlueprintId == triggerBlueprintId);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
           .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/output-data-variable-blueprints/{id}", async (GetOutputDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapPost("/output-data-variable-blueprints", async (CreateOutputDataVariableBlueprint command, OutputDataVariableBlueprint outputDataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(outputDataVariableBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-variable-blueprints/{outputDataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapPut("/output-data-variable-blueprints/{id}", async (UpdateOutputDataVariableBlueprint command, OutputDataVariableBlueprint updateOutputDataVariableBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<OutputDataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateOutputDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapDelete("/output-data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });
        }
    }
}