namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Persistence;
    using Ganss.Xss;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class ExpressionBlueprint : AuditableModel
    {
        private HtmlSanitizer sanitizer = new HtmlSanitizer();

        private string? objective;

        public string? Expression { get; set; }

        public string? Objective
        {
            get
            {
                return this.objective;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.objective = null;
                }
                else
                {
                    this.objective = this.sanitizer.Sanitize(value);
                }
            }
        }

        public Guid? BlueprintId { get; set; }

        public virtual Blueprint? Blueprint { get; set; }

        public virtual ICollection<InputDataVariableBlueprint>? InputDataVariableBlueprints { get; set; }

        public virtual ICollection<InputDataTableBlueprint>? InputDataTableBlueprints { get; set; }

        public Guid? OutputDataVariableBlueprintId { get; set; }

        public virtual OutputDataVariableBlueprint? OutputDataVariableBlueprint { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<ExpressionBlueprint>().ToTable(typeof(ExpressionBlueprint).Name.Pluralize());

            modelBuilder.Entity<ExpressionBlueprint>().HasKey(x => x.Id);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.CreatedById).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.LastUpdatedById).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.LastUpdatedBy).WithMany().HasForeignKey(x => x.LastUpdatedById).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpressionBlueprint>().Property(x => x.Expression).IsRequired();

            modelBuilder.Entity<ExpressionBlueprint>().HasMany(x => x.InputDataTableBlueprints).WithOne(x => x.ExpressionBlueprint).HasForeignKey(x => x.ExpressionBlueprintId);

            modelBuilder.Entity<ExpressionBlueprint>().HasMany(x => x.InputDataVariableBlueprints).WithOne(x => x.ExpressionBlueprint).HasForeignKey(x => x.ExpressionBlueprintId);

            modelBuilder.Entity<ExpressionBlueprint>().HasOne(x => x.OutputDataVariableBlueprint).WithOne(x => x.ExpressionBlueprint).HasForeignKey<ExpressionBlueprint>(x => x.OutputDataVariableBlueprintId);
        }
    }
}