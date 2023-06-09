namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class InputDataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/input-data-variable-blueprints", async (QueryInputDataVariableBlueprints query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints/{expressionBlueprintId}/input-data-variable-blueprints", async (QueryFilteredInputDataVariableBlueprints query, Guid expressionBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(skip, limit, sortBy, sortOrder, x => x.ExpressionBlueprintId == expressionBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/input-data-variable-blueprints", async (QueryFilteredInputDataVariableBlueprints query, Guid dataTableBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(skip, limit, sortBy, sortOrder, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/trigger-expression-blueprints/{triggerExpressionBlueprintId}/input-data-variable-blueprints", async (QueryFilteredInputDataVariableBlueprints query, Guid triggerExpressionBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(skip, limit, sortBy, sortOrder, x => x.TriggerExpressionBlueprintId == triggerExpressionBlueprintId);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
           .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
           .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/input-data-variable-blueprints/{id}", async (GetInputDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapPost("/input-data-variable-blueprints", async (CreateInputDataVariableBlueprint command, InputDataVariableBlueprint inputDataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(inputDataVariableBlueprint, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-variable-blueprints/{inputDataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapPut("/input-data-variable-blueprints/{id}", async (UpdateInputDataVariableBlueprint command, InputDataVariableBlueprint updateInputDataVariableBlueprint, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<InputDataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateInputDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapDelete("/input-data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithGroupName(ApiGroupNameConstants.InputDataVariableBlueprints)
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });
        }
    }
}