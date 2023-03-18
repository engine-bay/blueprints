namespace EngineBay.Blueprints
{
    using System;

    public class WorkbookDto
    {
        public WorkbookDto(Workbook workbook)
        {
            if (workbook is null)
            {
                throw new ArgumentNullException(nameof(workbook));
            }

            this.Id = workbook.Id;
            this.Name = workbook.Name;
            this.Description = workbook.Description;
            this.Blueprints = workbook.Blueprints?.Select(x => new BlueprintDto(x)).ToList();
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public IEnumerable<BlueprintDto>? Blueprints { get; set; }
    }
}