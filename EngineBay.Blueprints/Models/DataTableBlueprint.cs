namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataTableBlueprint : AuditableModel
    {
        private HtmlSanitizer sanitizer = new HtmlSanitizer();

        private string? description;

        public string? Namespace { get; set; }

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

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public virtual ICollection<InputDataVariableBlueprint>? InputDataVariableBlueprints { get; set; }

        public virtual ICollection<DataTableColumnBlueprint>? DataTableColumnBlueprints { get; set; }

        public virtual ICollection<DataTableRowBlueprint>? DataTableRowBlueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<DataTableBlueprint>().ToTable(typeof(DataTableBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataTableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<DataTableBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId }).IsUnique();

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.DataTableColumnBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.InputDataVariableBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);

            modelBuilder.Entity<DataTableBlueprint>().HasMany(x => x.DataTableRowBlueprints).WithOne(x => x.DataTableBlueprint).HasForeignKey(x => x.DataTableBlueprintId);
        }
    }
}