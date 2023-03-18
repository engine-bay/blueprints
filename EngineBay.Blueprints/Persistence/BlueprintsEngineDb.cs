namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class BlueprintsEngineDb : EngineWriteDb, IEngineWriteDb
    {
        public BlueprintsEngineDb(DbContextOptions<EngineWriteDb> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public DbSet<Workbook> Workbooks { get; set; } = null!;

        /// <inheritdoc/>
        public DbSet<Blueprint> Blueprints { get; set; } = null!;

        /// <inheritdoc/>
        public DbSet<DataVariableBlueprint> DataVariableBlueprints { get; set; } = null!;

        public DbSet<DataTableBlueprint> DataTableBlueprints { get; set; } = null!;

        public DbSet<DataTableColumnBlueprint> DataTableColumnBlueprints { get; set; } = null!;

        public DbSet<DataTableRowBlueprint> DataTableRowBlueprints { get; set; } = null!;

        public DbSet<DataTableCellBlueprint> DataTableCellBlueprints { get; set; } = null!;

        /// <inheritdoc/>
        public DbSet<ExpressionBlueprint> ExpressionBlueprints { get; set; } = null!;

        public DbSet<InputDataTableBlueprint> InputDataTableBlueprints { get; set; } = null!;

        public DbSet<InputDataVariableBlueprint> InputDataVariableBlueprints { get; set; } = null!;

        public DbSet<OutputDataVariableBlueprint> OutputDataVariableBlueprints { get; set; } = null!;

        public DbSet<TriggerBlueprint> TriggerBlueprints { get; set; } = null!;

        public DbSet<TriggerExpressionBlueprint> TriggerExpressionBlueprints { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Workbook.CreateDataAnnotations(modelBuilder);
            Blueprint.CreateDataAnnotations(modelBuilder);
            DataVariableBlueprint.CreateDataAnnotations(modelBuilder);
            DataTableBlueprint.CreateDataAnnotations(modelBuilder);
            DataTableColumnBlueprint.CreateDataAnnotations(modelBuilder);
            DataTableRowBlueprint.CreateDataAnnotations(modelBuilder);
            DataTableCellBlueprint.CreateDataAnnotations(modelBuilder);
            ExpressionBlueprint.CreateDataAnnotations(modelBuilder);
            InputDataTableBlueprint.CreateDataAnnotations(modelBuilder);
            InputDataVariableBlueprint.CreateDataAnnotations(modelBuilder);
            OutputDataVariableBlueprint.CreateDataAnnotations(modelBuilder);
            TriggerBlueprint.CreateDataAnnotations(modelBuilder);
            TriggerExpressionBlueprint.CreateDataAnnotations(modelBuilder);
        }
    }
}