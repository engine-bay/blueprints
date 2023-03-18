namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class Workbook : BaseModel
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

            modelBuilder.Entity<Workbook>().HasKey(x => x.Id);

            modelBuilder.Entity<Workbook>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<Workbook>().Property(x => x.LastUpdatedAt).IsRequired();
            modelBuilder.Entity<Workbook>().Property(x => x.Name).IsRequired();

            // modelBuilder.Entity<Workbook>().HasIndex(x => x.Name).IsUnique(); // disabled for now till a concept of data ownership can be introduced.
            modelBuilder.Entity<Workbook>().HasMany(x => x.Blueprints);
        }
    }
}