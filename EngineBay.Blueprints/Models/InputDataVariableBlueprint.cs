namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class InputDataVariableBlueprint : AuditableModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public virtual ExpressionBlueprint? ExpressionBlueprint { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

        public Guid? TriggerExpressionBlueprintId { get; set; }

        public virtual TriggerExpressionBlueprint? TriggerExpressionBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<InputDataVariableBlueprint>().ToTable(typeof(InputDataVariableBlueprint).Name.Pluralize());

            modelBuilder.Entity<InputDataVariableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.ExpressionBlueprint);

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.DataTableBlueprint);

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.TriggerExpressionBlueprint);
        }
    }
}