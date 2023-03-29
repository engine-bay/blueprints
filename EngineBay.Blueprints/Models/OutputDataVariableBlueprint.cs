namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class OutputDataVariableBlueprint : AuditableModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public virtual ExpressionBlueprint? ExpressionBlueprint { get; set; }

        public Guid? TriggerBlueprintId { get; set; }

        public virtual TriggerBlueprint? TriggerBlueprint { get; set; }

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

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasIndex(x => new { x.Name, x.ExpressionBlueprintId, x.TriggerBlueprintId, x.Namespace }).IsUnique();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasOne(x => x.ExpressionBlueprint);

            modelBuilder.Entity<OutputDataVariableBlueprint>().HasOne(x => x.TriggerBlueprint);
        }
    }
}