namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class OutputDataVariableBlueprint : BaseModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public virtual ExpressionBlueprint? ExpressionBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<OutputDataVariableBlueprint>().ToTable(typeof(OutputDataVariableBlueprint).Name.Pluralize());

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();
            modelBuilder.Entity<OutputDataVariableBlueprint>().HasIndex(x => new { x.Name, x.ExpressionBlueprintId, x.Namespace }).IsUnique();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasOne(x => x.ExpressionBlueprint);
        }
    }
}