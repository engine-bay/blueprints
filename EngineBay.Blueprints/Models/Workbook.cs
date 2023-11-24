namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class Workbook : AuditableModel
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

        public virtual ICollection<Blueprint>? Blueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<Workbook>().ToTable(typeof(Workbook).Name.Pluralize());

            modelBuilder.Entity<Workbook>().HasKey(x => x.Id);

            modelBuilder.Entity<Workbook>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<Workbook>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<Workbook>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<Workbook>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workbook>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<Workbook>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workbook>().Property(x => x.Name).IsRequired();

            // modelBuilder.Entity<Workbook>().HasIndex(x => x.Name).IsUnique(); // disabled for now till a concept of data ownership can be introduced.
            modelBuilder.Entity<Workbook>().HasMany(x => x.Blueprints);
        }
    }
}