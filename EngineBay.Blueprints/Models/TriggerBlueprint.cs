namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TriggerBlueprint : BaseModel
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<TriggerExpressionBlueprint>? TriggerExpressionBlueprints { get; set; }

        public Guid OutputDataVariableBlueprintId { get; set; }

        public virtual OutputDataVariableBlueprint? OutputDataVariableBlueprint { get; set; }

        public Guid BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<TriggerBlueprint>().ToTable(typeof(TriggerBlueprint).Name.Pluralize());

            modelBuilder.Entity<TriggerBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId, x.OutputDataVariableBlueprintId }).IsUnique();

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.Blueprint);

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.OutputDataVariableBlueprint);

            modelBuilder.Entity<TriggerBlueprint>().HasMany(x => x.TriggerExpressionBlueprints).WithOne(x => x.TriggerBlueprint).HasForeignKey(x => x.TriggerBlueprintId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}