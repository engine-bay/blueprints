namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class DataVariableBlueprint : BaseModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public string? DefaultValue { get; set; }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<DataVariableBlueprint>().HasKey(x => x.Id);
            modelBuilder.Entity<DataVariableBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId, x.Namespace }).IsUnique();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();
            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Namespace).IsRequired();
        }
    }
}