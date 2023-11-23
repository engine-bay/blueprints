namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class DataVariableBlueprint : AuditableModel
    {
        private HtmlSanitizer sanitizer = new HtmlSanitizer();

        private string? description;

        public string? Name { get; set; }

        public string? Namespace { get; set; }

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

        public string? Type { get; set; }

        public string? DefaultValue { get; set; }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<DataVariableBlueprint>().ToTable(typeof(DataVariableBlueprint).Name.Pluralize());

            modelBuilder.Entity<DataVariableBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataVariableBlueprint>().HasIndex(x => new { x.Name, x.BlueprintId, x.Namespace }).IsUnique();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<DataVariableBlueprint>().Property(x => x.Namespace).IsRequired();
        }
    }
}