namespace EngineBay.Blueprints
{
    using System;

    public class WorkbookMetaDataDto
    {
        public WorkbookMetaDataDto(Workbook workbook)
        {
            if (workbook is null)
            {
                throw new ArgumentNullException(nameof(workbook));
            }

            this.Id = workbook.Id;
            this.Name = workbook.Name;
            this.Description = workbook.Description;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}