namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class Workbook : AuditableModel
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Blueprint>? Blueprints { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

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