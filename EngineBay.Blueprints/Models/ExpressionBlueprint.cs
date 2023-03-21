namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class ExpressionBlueprint : AuditableModel
    {
        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public virtual ICollection<InputDataVariableBlueprint>? InputDataVariableBlueprints { get; set; }

        public virtual ICollection<InputDataTableBlueprint>? InputDataTableBlueprints { get; set; }

        public Guid OutputDataVariableBlueprintId { get; set; }

        public virtual OutputDataVariableBlueprint? OutputDataVariableBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ExpressionBlueprint>().ToTable(typeof(ExpressionBlueprint).Name.Pluralize());

            modelBuilder.Entity<ExpressionBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.CreatedBy);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.LastUpdatedBy);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.Expression).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasMany(x => x.InputDataTableBlueprints).WithOne(x => x.ExpressionBlueprint).HasForeignKey(x => x.ExpressionBlueprintId);

            modelBuilder.Entity<ExpressionBlueprint>().HasMany(x => x.InputDataVariableBlueprints).WithOne(x => x.ExpressionBlueprint).HasForeignKey(x => x.ExpressionBlueprintId);

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.OutputDataVariableBlueprint).WithOne(x => x.ExpressionBlueprint).HasForeignKey<ExpressionBlueprint>(x => x.OutputDataVariableBlueprintId);
        }
    }
}