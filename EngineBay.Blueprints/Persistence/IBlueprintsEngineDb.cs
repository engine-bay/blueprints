namespace EngineBay.Blueprints
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public interface IBlueprintsEngineDb : IEngineDb
    {
        DbSet<Workbook> Workbooks { get; set; }

        DbSet<Blueprint> Blueprints { get; set; }

        DbSet<DataVariableBlueprint> DataVariableBlueprints { get; set; }

        DbSet<DataTableBlueprint> DataTableBlueprints { get; set; }

        DbSet<DataTableColumnBlueprint> DataTableColumnBlueprints { get; set; }

        DbSet<DataTableRowBlueprint> DataTableRowBlueprints { get; set; }

        DbSet<DataTableCellBlueprint> DataTableCellBlueprints { get; set; }

        DbSet<ExpressionBlueprint> ExpressionBlueprints { get; set; }

        DbSet<InputDataTableBlueprint> InputDataTableBlueprints { get; set; }

        DbSet<InputDataVariableBlueprint> InputDataVariableBlueprints { get; set; }

        DbSet<OutputDataVariableBlueprint> OutputDataVariableBlueprints { get; set; }

        DbSet<TriggerBlueprint> TriggerBlueprints { get; set; }

        DbSet<TriggerExpressionBlueprint> TriggerExpressionBlueprints { get; set; }
    }
}