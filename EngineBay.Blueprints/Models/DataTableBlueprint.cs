namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableBlueprint : AuditableModel
    {
        public string? Namespace { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public virtual ICollection<InputDataVariableBlueprint>? InputDataVariableBlueprints { get; set; }

        public virtual ICollection<DataTableColumnBlueprint>? DataTableColumnBlueprints { get; set; }

        public virtual ICollection<DataTableRowBlueprint>? DataTableRowBlueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataTableBlueprint>().ToTable(typeof(DataTableBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId }).IsUnique();

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.DataTableColumnBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.InputDataVariableBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.DataTableRowBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);
        }
    }
}