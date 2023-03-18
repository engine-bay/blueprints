namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class BlueprintsModule : IModule
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            // Register commands
            services.AddTransient<CreateBlueprint>();
            services.AddTransient<UpdateBlueprint>();
            services.AddTransient<DeleteBlueprint>();
            services.AddTransient<CreateDataVariableBlueprint>();
            services.AddTransient<DeleteDataVariableBlueprint>();
            services.AddTransient<UpdateDataVariableBlueprint>();
            services.AddTransient<CreateExpressionBlueprint>();
            services.AddTransient<DeleteExpressionBlueprint>();
            services.AddTransient<UpdateExpressionBlueprint>();
            services.AddTransient<CreateWorkbook>();
            services.AddTransient<DeleteWorkbook>();
            services.AddTransient<UpdateWorkbook>();

            // Register queries
            services.AddTransient<QueryBlueprints>();
            services.AddTransient<GetBlueprint>();
            services.AddTransient<GetDataVariableBlueprint>();
            services.AddTransient<QueryDataVariableBlueprints>();
            services.AddTransient<GetExpressionBlueprint>();
            services.AddTransient<QueryExpressionBlueprints>();
            services.AddTransient<GetWorkbook>();
            services.AddTransient<QueryWorkbooks>();
            services.AddTransient<GetWorkbookComplexityScore>();

            // Register validators
            services.AddTransient<IValidator<Workbook>, WorkbookValidator>();
            services.AddTransient<IValidator<Blueprint>, BlueprintValidator>();
            services.AddTransient<IValidator<ExpressionBlueprint>, ExpressionBlueprintValidator>();
            services.AddTransient<IValidator<DataVariableBlueprint>, DataVariableBlueprintValidator>();
            services.AddTransient<IValidator<DataTableBlueprint>, DataTableBlueprintValidator>();
            services.AddTransient<IValidator<DataTableColumnBlueprint>, DataTableColumnBlueprintValidator>();
            services.AddTransient<IValidator<DataTableRowBlueprint>, DataTableRowBlueprintValidator>();
            services.AddTransient<IValidator<DataTableCellBlueprint>, DataTableCellBlueprintValidator>();
            services.AddTransient<IValidator<TriggerBlueprint>, TriggerBlueprintValidator>();
            services.AddTransient<IValidator<TriggerExpressionBlueprint>, TriggerExpressionBlueprintValidator>();
            services.AddTransient<IValidator<InputDataVariableBlueprint>, InputDataVariableBlueprintValidator>();
            services.AddTransient<IValidator<InputDataTableBlueprint>, InputDataTableBlueprintValidator>();
            services.AddTransient<IValidator<OutputDataVariableBlueprint>, OutputDataVariableBlueprintValidator>();

            // register persistence services
            var databaseConfiguration = new DatabaseConfiguration<BlueprintsEngineDb, BlueprintsEngineQueryDb, BlueprintsEngineWriteDb>();
            databaseConfiguration.RegisterDatabases(services);

            return services;
        }

        /// <inheritdoc/>
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            // blueprints
            endpoints.MapGet("/blueprints", async (QueryBlueprints query, int? skip, int? limit, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters
                {
                    Skip = skip,
                    Limit = limit,
                };

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            });

            endpoints.MapGet("/blueprints/{id}", async (GetBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapPost("/blueprints", async (CreateBlueprint command, Blueprint blueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(blueprint, cancellation).ConfigureAwait(false);
                return Results.Created($"/blueprints/{blueprint.Id}", dto);
            });

            endpoints.MapPut("/blueprints/{id}", async (UpdateBlueprint command, Blueprint updateBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Blueprint>
                {
                    Id = id,
                    Entity = updateBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapDelete("/blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            // data-variable-blueprints
            endpoints.MapGet("/data-variable-blueprints", async (QueryDataVariableBlueprints query, int? skip, int? limit, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters
                {
                    Skip = skip,
                    Limit = limit,
                };

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            });

            endpoints.MapGet("/data-variable-blueprints/{id}", async (GetDataVariableBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapPost("/data-variable-blueprints", async (CreateDataVariableBlueprint command, DataVariableBlueprint dataVariableBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(dataVariableBlueprint, cancellation).ConfigureAwait(false);
                return Results.Created($"/data-variable-blueprints/{dataVariableBlueprint.Id}", dto);
            });

            endpoints.MapPut("/data-variable-blueprints/{id}", async (UpdateDataVariableBlueprint command, DataVariableBlueprint updateDataVariableBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<DataVariableBlueprint>
                {
                    Id = id,
                    Entity = updateDataVariableBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapDelete("/data-variable-blueprints/{id}", async (DeleteBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            // expression-blueprints
            endpoints.MapGet("/expression-blueprints", async (QueryExpressionBlueprints query, int? skip, int? limit, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters
                {
                    Skip = skip,
                    Limit = limit,
                };

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            });

            endpoints.MapGet("/expression-blueprints/{id}", async (GetExpressionBlueprint query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapPost("/expression-blueprints", async (CreateExpressionBlueprint command, ExpressionBlueprint expressionBlueprint, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(expressionBlueprint, cancellation).ConfigureAwait(false);
                return Results.Created($"/expression-blueprints/{expressionBlueprint.Id}", dto);
            });

            endpoints.MapPut("/expression-blueprints/{id}", async (UpdateExpressionBlueprint command, ExpressionBlueprint updateExpressionBlueprint, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<ExpressionBlueprint>
                {
                    Id = id,
                    Entity = updateExpressionBlueprint,
                };
                var dto = await command.Handle(updateParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapDelete("/expression-blueprints/{id}", async (DeleteExpressionBlueprint command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            // workbooks
            endpoints.MapGet("/workbooks", async (QueryWorkbooks query, int? skip, int? limit, CancellationToken cancellation) =>
            {
                var paginationParameters = new PaginationParameters
                {
                    Skip = skip,
                    Limit = limit,
                };

                var paginatedDtos = await query.Handle(paginationParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(paginatedDtos);
            });

            endpoints.MapGet("/workbooks/{id}", async (GetWorkbook query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapGet("/workbooks/{id}/complexity-score", async (GetWorkbookComplexityScore query, Guid id, CancellationToken cancellation) =>
            {
                var dto = await query.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapPost("/workbooks", async (CreateWorkbook command, Workbook workbook, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(workbook, cancellation).ConfigureAwait(false);
                return Results.Created($"/workbooks/{workbook.Id}", dto);
            });

            endpoints.MapPut("/workbooks/{id}", async (UpdateWorkbook command, Workbook updateWorkbook, Guid id, CancellationToken cancellation) =>
            {
                var updateParameters = new UpdateParameters<Workbook>
                {
                    Id = id,
                    Entity = updateWorkbook,
                };
                var dto = await command.Handle(updateParameters, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            endpoints.MapDelete("/workbooks/{id}", async (DeleteWorkbook command, Guid id, CancellationToken cancellation) =>
            {
                var dto = await command.Handle(id, cancellation).ConfigureAwait(false);
                return Results.Ok(dto);
            });

            return endpoints;
        }
    }
}