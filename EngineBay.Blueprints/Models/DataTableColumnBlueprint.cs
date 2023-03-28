namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableColumnBlueprint : AuditableModel
    {
        public string? Name { get; set; }

        public string? Type { get; set; }

        public Guid DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataTableColumnBlueprint>().ToTable(typeof(DataTableColumnBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableColumnBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().HasIndex(x => new { x.Name, x.DataTableBlueprintId }).IsUnique();
        }
    }
}