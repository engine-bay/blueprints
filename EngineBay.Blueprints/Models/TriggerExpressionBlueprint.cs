namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TriggerExpressionBlueprint : AuditableModel
    {
        public string? Expression { get; set; }

        public string? Objective { get; set; }

        public Guid TriggerBlueprintId { get; set; }

        public virtual TriggerBlueprint? TriggerBlueprint { get; set; }

        public Guid InputDataVariableBlueprintId { get; set; }

        public virtual InputDataVariableBlueprint? InputDataVariableBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<TriggerExpressionBlueprint>().ToTable(typeof(TriggerExpressionBlueprint).Name.Pluralize());

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<TriggerExpressionBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TriggerExpressionBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TriggerExpressionBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TriggerExpressionBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TriggerExpressionBlueprint>().Property(x => x.Expression).IsRequired();

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasIndex(x => new { x.Expression, x.TriggerBlueprintId, x.InputDataVariableBlueprintId }).IsUnique();

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasOne(x => x.InputDataVariableBlueprint);

            modelBuilder.Entity<TriggerExpressionBlueprint>().HasOne(x => x.TriggerBlueprint);
        }
    }
}