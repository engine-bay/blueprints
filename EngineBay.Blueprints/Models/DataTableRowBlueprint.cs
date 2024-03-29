namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableRowBlueprint : AuditableModel
    {
        public Guid? DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

        public virtual ICollection<DataTableCellBlueprint>? DataTableCellBlueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<DataTableRowBlueprint>().ToTable(typeof(DataTableRowBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableRowBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<DataTableRowBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<DataTableRowBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableRowBlueprint>().HasMany(x => x.DataTableCellBlueprints).WithOne(x => x.DataTableRowBlueprint);
        }
    }
}