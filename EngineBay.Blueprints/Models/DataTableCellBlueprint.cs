namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableCellBlueprint : AuditableModel
    {
        public string? Key { get; set; }

        public string? Value { get; set; }

        public string? Namespace { get; set; }

        public string? Name { get; set; }

        public Guid? DataTableRowBlueprintId { get; set; }

        public virtual DataTableRowBlueprint? DataTableRowBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataTableCellBlueprint>().ToTable(typeof(DataTableCellBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableCellBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.Key).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.Value).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataTableCellBlueprint>().Property(x => x.Namespace).IsRequired();
        }
    }
}