namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class InputDataVariableBlueprint : BaseModel
    {
        public string? Name { get; set; }

        public string? Namespace { get; set; }

        public string? Type { get; set; }

        public Guid? ExpressionBlueprintId { get; set; }

        public virtual ExpressionBlueprint? ExpressionBlueprint { get; set; }

        public Guid? DataTableBlueprintId { get; set; }

        public virtual DataTableBlueprint? DataTableBlueprint { get; set; }

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

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Namespace).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().Property(x => x.Type).IsRequired();

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.ExpressionBlueprint);

            modelBuilder.Entity<InputDataVariableBlueprint>().HasOne(x => x.DataTableBlueprint);
        }
    }
}