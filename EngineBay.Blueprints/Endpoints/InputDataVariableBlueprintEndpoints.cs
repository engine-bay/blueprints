namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class InputDataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/input-data-variable-blueprints", async (QueryInputDataVariableBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints/{expressionBlueprintId}/input-data-variable-blueprints", async (QueryInputDataVariableBlueprints query, Guid expressionBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(searchParameters, x => x.ExpressionBlueprintId == expressionBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/data-table-blueprints/{dataTableBlueprintId}/input-data-variable-blueprints", async (QueryInputDataVariableBlueprints query, Guid dataTableBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(searchParameters, x => x.DataTableBlueprintId == dataTableBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/trigger-expression-blueprints/{triggerExpressionBlueprintId}/input-data-variable-blueprints", async (QueryInputDataVariableBlueprints query, Guid triggerExpressionBlueprintId, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<InputDataVariableBlueprint>(paginationParameters, x => x.TriggerExpressionBlueprintId == triggerExpressionBlueprintId);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
           .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapGet("/input-data-variable-blueprints/{id}", async (GetInputDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapPost("/input-data-variable-blueprints", async (CreateInputDataVariableBlueprint command, InputDataVariableBlueprint inputDataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(inputDataVariableBlueprint, claimsPrincipal, cancellation);
                return Results.Created($"/data-variable-blueprints/{inputDataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
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
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });

            endpoints.MapDelete("/input-data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.InputDataVariableBlueprints,
            });
        }
    }
}