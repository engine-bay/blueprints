namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class BlueprintsModule : IModule
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
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
            services.AddTransient<CreateDataTableBlueprint>();
            services.AddTransient<DeleteDataTableBlueprint>();
            services.AddTransient<UpdateDataTableBlueprint>();
            services.AddTransient<CreateInputDataVariableBlueprint>();
            services.AddTransient<DeleteInputDataVariableBlueprint>();
            services.AddTransient<UpdateInputDataVariableBlueprint>();
            services.AddTransient<CreateOutputDataVariableBlueprint>();
            services.AddTransient<DeleteOutputDataVariableBlueprint>();
            services.AddTransient<UpdateOutputDataVariableBlueprint>();
            services.AddTransient<CreateDataTableColumnBlueprint>();
            services.AddTransient<DeleteDataTableColumnBlueprint>();
            services.AddTransient<UpdateDataTableColumnBlueprint>();
            services.AddTransient<CreateDataTableRowBlueprint>();
            services.AddTransient<DeleteDataTableRowBlueprint>();
            services.AddTransient<UpdateDataTableRowBlueprint>();
            services.AddTransient<CreateDataTableCellBlueprint>();
            services.AddTransient<DeleteDataTableCellBlueprint>();
            services.AddTransient<UpdateDataTableCellBlueprint>();
            services.AddTransient<CreateTriggerBlueprint>();
            services.AddTransient<DeleteTriggerBlueprint>();
            services.AddTransient<UpdateTriggerBlueprint>();
            services.AddTransient<CreateTriggerExpressionBlueprint>();
            services.AddTransient<DeleteTriggerExpressionBlueprint>();
            services.AddTransient<UpdateTriggerExpressionBlueprint>();
            services.AddTransient<CreateWorkbook>();
            services.AddTransient<DeleteWorkbook>();
            services.AddTransient<UpdateWorkbook>();

            // Register queries
            services.AddTransient<QueryBlueprints>();
            services.AddTransient<QueryFilteredBlueprints>();
            services.AddTransient<GetBlueprint>();
            services.AddTransient<GetDataVariableBlueprint>();
            services.AddTransient<QueryDataVariableBlueprints>();
            services.AddTransient<QueryFilteredDataVariableBlueprints>();
            services.AddTransient<GetExpressionBlueprint>();
            services.AddTransient<QueryExpressionBlueprints>();
            services.AddTransient<QueryFilteredExpressionBlueprints>();
            services.AddTransient<GetDataTableBlueprint>();
            services.AddTransient<QueryDataTableBlueprints>();
            services.AddTransient<QueryFilteredDataTableBlueprints>();
            services.AddTransient<GetWorkbook>();
            services.AddTransient<QueryWorkbooks>();
            services.AddTransient<GetWorkbookComplexityScore>();
            services.AddTransient<GetInputDataVariableBlueprint>();
            services.AddTransient<QueryInputDataVariableBlueprints>();
            services.AddTransient<QueryFilteredDataVariableBlueprints>();
            services.AddTransient<QueryFilteredDataTableColumnBlueprints>();
            services.AddTransient<GetDataTableColumnBlueprint>();
            services.AddTransient<QueryDataTableColumnBlueprints>();
            services.AddTransient<QueryFilteredDataTableRowBlueprints>();
            services.AddTransient<GetDataTableRowBlueprint>();
            services.AddTransient<QueryDataTableRowBlueprints>();
            services.AddTransient<QueryFilteredDataTableCellBlueprints>();
            services.AddTransient<GetDataTableCellBlueprint>();
            services.AddTransient<QueryDataTableCellBlueprints>();
            services.AddTransient<QueryTriggerBlueprints>();
            services.AddTransient<QueryFilteredTriggerBlueprints>();
            services.AddTransient<GetTriggerBlueprint>();
            services.AddTransient<QueryTriggerExpressionBlueprints>();
            services.AddTransient<QueryFilteredTriggerExpressionBlueprints>();
            services.AddTransient<GetTriggerExpressionBlueprint>();
            services.AddTransient<QueryOutputDataVariableBlueprints>();
            services.AddTransient<QueryFilteredOutputDataVariableBlueprints>();
            services.AddTransient<GetOutputDataVariableBlueprint>();

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
            var databaseConfiguration = new CQRSDatabaseConfiguration<BlueprintsDbContext, BlueprintsQueryDbContext, BlueprintsWriteDbContext>();
            databaseConfiguration.RegisterDatabases(services);

            return services;
        }

        /// <inheritdoc/>
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            BlueprintEndpoints.MapEndpoints(endpoints);
            DataVariableBlueprintEndpoints.MapEndpoints(endpoints);
            DataTableBlueprintEndpoints.MapEndpoints(endpoints);
            ExpressionBlueprints.MapEndpoints(endpoints);
            WorkbookEndpoints.MapEndpoints(endpoints);
            InputDataVariableBlueprintEndpoints.MapEndpoints(endpoints);
            OutputDataVariableBlueprintEndpoints.MapEndpoints(endpoints);
            DataTableColumnBlueprintEndpoints.MapEndpoints(endpoints);
            DataTableRowBlueprintEndpoints.MapEndpoints(endpoints);
            DataTableCellBlueprintEndpoints.MapEndpoints(endpoints);
            TriggerBlueprints.MapEndpoints(endpoints);
            TriggerExpressionBlueprints.MapEndpoints(endpoints);

            return endpoints;
        }

        public WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }
    }
}