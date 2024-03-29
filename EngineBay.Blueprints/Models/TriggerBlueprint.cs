namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TriggerBlueprint : AuditableModel
    {
        private HtmlSanitizer sanitizer = new HtmlSanitizer();

        private string? description;

        public string? Name { get; set; }

        public string? Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.description = null;
                }
                else
                {
                    this.description = this.sanitizer.Sanitize(value);
                }
            }
        }

        public virtual ICollection<TriggerExpressionBlueprint>? TriggerExpressionBlueprints { get; set; }

        public Guid? OutputDataVariableBlueprintId { get; set; }

        public virtual OutputDataVariableBlueprint? OutputDataVariableBlueprint { get; set; }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<TriggerBlueprint>().ToTable(typeof(TriggerBlueprint).Name.Pluralize());

            modelBuilder.Entity<TriggerBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TriggerBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<TriggerBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId, x.OutputDataVariableBlueprintId }).IsUnique();

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.Blueprint);

            modelBuilder.Entity<TriggerBlueprint>().HasOne(x => x.OutputDataVariableBlueprint).WithOne(x => x.TriggerBlueprint).HasForeignKey<OutputDataVariableBlueprint>(x => x.TriggerBlueprintId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TriggerBlueprint>().HasMany(x => x.TriggerExpressionBlueprints).WithOne(x => x.TriggerBlueprint).HasForeignKey(x => x.TriggerBlueprintId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}