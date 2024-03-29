namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class Blueprint : AuditableModel
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

        public virtual ICollection<ExpressionBlueprint>? ExpressionBlueprints { get; set; }

        public virtual ICollection<DataVariableBlueprint>? DataVariableBlueprints { get; set; }

        public virtual ICollection<DataTableBlueprint>? DataTableBlueprints { get; set; }

        public virtual ICollection<TriggerBlueprint>? TriggerBlueprints { get; set; }

        public Guid? WorkbookId { get; set; }

        public virtual Workbook? Workbook { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<Blueprint>().ToTable(typeof(Blueprint).Name.Pluralize());

            modelBuilder.Entity<Blueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<Blueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<Blueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<Blueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<Blueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Blueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<Blueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Blueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<Blueprint>().HasIndex(x => new { x.Name, x.WorkbookId }).IsUnique();

            modelBuilder.Entity<Blueprint>().HasOne(x => x.Workbook).WithMany(x => x.Blueprints).HasForeignKey(x => x.WorkbookId);

            modelBuilder.Entity<Blueprint>().HasMany(x => x.ExpressionBlueprints).WithOne(x => x.Blueprint).HasForeignKey(x => x.BlueprintId);

            modelBuilder.Entity<Blueprint>().HasMany(x => x.TriggerBlueprints).WithOne(x => x.Blueprint).HasForeignKey(x => x.BlueprintId);

            modelBuilder.Entity<Blueprint>().HasMany(x => x.DataVariableBlueprints).WithOne(x => x.Blueprint).HasForeignKey(x => x.BlueprintId);

            modelBuilder.Entity<Blueprint>().HasMany(x => x.DataTableBlueprints).WithOne(x => x.Blueprint).HasForeignKey(x => x.BlueprintId);
        }
    }
}