namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class InputDataTableBlueprint : AuditableModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public virtual ExpressionBlueprint? ExpressionBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<InputDataTableBlueprint>().ToTable(typeof(InputDataTableBlueprint).Name.Pluralize());

            modelBuilder.Entity<InputDataTableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InputDataTableBlueprint>().HasIndex(x => new { x.Name, x.Namespace, x.ExpressionBlueprintId }).IsUnique();

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<InputDataTableBlueprint>().HasOne(x => x.ExpressionBlueprint);
        }
    }
}