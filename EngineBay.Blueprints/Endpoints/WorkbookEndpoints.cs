namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public static class WorkbookEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/workbooks", async (QueryWorkbooks query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>(searchParameters, filter);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapGet("/workbooks/{id}", async (GetWorkbook query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapGet("/workbooks/{id}/complexity-score", async (GetWorkbookComplexityScore query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapPost("/workbooks", async (CreateWorkbook command, Workbook workbook, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(workbook, cancellation);
                return Results.Created($"/workbooks/{workbook.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapPut("/workbooks/{id}", async (UpdateWorkbook command, Workbook updateWorkbook, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Workbook>
                {
                    Id = id,
                    Entity = updateWorkbook,
                };
                var dto = await command.Handle(updateParameters, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapDelete("/workbooks/{id}", async (DeleteWorkbook command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });
        }
    }
}