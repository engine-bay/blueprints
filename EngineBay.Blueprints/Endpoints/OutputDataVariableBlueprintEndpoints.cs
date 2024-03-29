namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;

    public static class OutputDataVariableBlueprintEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(searchParameters, filter);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/expression-blueprints/{expressionBlueprintId}/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, Guid expressionBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
            {
                var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
                var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(searchParameters, x => x.ExpressionBlueprintId == expressionBlueprintId);

                var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
                return Results.Ok(paginatedDtos);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/workbooks/{workbookId}/blueprints/{blueprintId}/trigger-blueprints/{triggerBlueprintId}/output-data-variable-blueprints", async (QueryOutputDataVariableBlueprints query, Guid triggerBlueprintId, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<OutputDataVariableBlueprint>(searchParameters, x => x.TriggerBlueprintId == triggerBlueprintId);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
           .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapGet("/output-data-variable-blueprints/{id}", async (GetOutputDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapPost("/output-data-variable-blueprints", async (CreateOutputDataVariableBlueprint command, OutputDataVariableBlueprint outputDataVariableBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(outputDataVariableBlueprint, cancellation);
                return Results.Created($"/data-variable-blueprints/{outputDataVariableBlueprint.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapPut("/output-data-variable-blueprints/{id}", async (UpdateOutputDataVariableBlueprint command, OutputDataVariableBlueprint updateOutputDataVariableBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<OutputDataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateOutputDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });

            endpoints.MapDelete("/output-data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.OutputDataVariableBlueprints,
            });
        }
    }
}