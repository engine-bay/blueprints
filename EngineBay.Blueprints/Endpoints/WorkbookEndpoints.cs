namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class WorkbookEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/workbooks", async (QueryWorkbooks query, FilterParameters? filter, string? search, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
           {
               var searchParameters = new SearchParameters(search, skip, limit, sortBy, sortOrder);
               var filteredPaginationParameters = new FilteredPaginationParameters<Workbook>(searchParameters, filter);

               var paginatedDtos = await query.Handle(filteredPaginationParameters, cancellation).ConfigureAwait(false);
               return Results.Ok(paginatedDtos);
           }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapGet("/workbooks/{id}", async (GetWorkbook query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapGet("/workbooks/{id}/complexity-score", async (GetWorkbookComplexityScore query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapPost("/workbooks", async (CreateWorkbook command, Workbook workbook, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(workbook, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Created($"/workbooks/{workbook.Id}", dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapPut("/workbooks/{id}", async (UpdateWorkbook command, Workbook updateWorkbook, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Workbook>
                {
                    Id = id,
                    Entity = updateWorkbook,
                };
                var dto = await command.Handle(updateParameters, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });

            endpoints.MapDelete("/workbooks/{id}", async (DeleteWorkbook command, Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, claimsPrincipal, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            }).RequireAuthorization()
            .WithTags(new string[]
            {
                ApiGroupNameConstants.Workbooks,
            });
        }
    }
}