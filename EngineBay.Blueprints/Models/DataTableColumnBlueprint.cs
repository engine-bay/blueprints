namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class DataTableColumnBlueprint : BaseModel
    {
        public string? Name { get; set; }

        public string? Type { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataTableColumnBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();
            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<DataTableColumnBlueprint>().HasIndex(x => new { x.Name, x.DataTableBlueprintId }).IsUnique();
        }
    }
}