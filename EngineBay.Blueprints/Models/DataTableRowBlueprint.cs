namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableRowBlueprint : BaseModel
    {
        public Guid? DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

        public virtual ICollection<DataTableCellBlueprint>? DataTableCellBlueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataTableRowBlueprint>().ToTable(typeof(DataTableRowBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableRowBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableRowBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();
            modelBuilder.Entity<DataTableRowBlueprint>().HasMany(x => x.DataTableCellBlueprints).WithOne(x => x.DataTableRowBlueprint);
        }
    }
}